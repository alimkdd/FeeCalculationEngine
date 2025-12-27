using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Dtos.Responses;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces;
using FeeCalculationEngine.Application.Interfaces.FeeCalculators;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Services.FeeModifiers;
using FeeCalculationEngine.Domain.Models;
using FeeCalculationEngine.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TransactionType = FeeCalculationEngine.Application.Enums.TransactionType;

namespace FeeCalculationEngine.Application.Services;

public class FeeCalculationService : IFeeCalculationService
{
    private readonly AppDbContext _dbContext;

    private readonly IDomesticTransferFeeCalculator _domestic;
    private readonly IInternationalTransferFeeCalculator _international;
    private readonly IWithdrawalFeeCalculator _withdrawal;
    private readonly IDepositFeeCalculator _deposit;

    private readonly IConfiguration _configuration;

    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<FeeCalculationService> _logger;

    private readonly ModifierStackingStrategy _stackingStrategy;

    public FeeCalculationService(
        AppDbContext dbContext,
        IDomesticTransferFeeCalculator domestic,
        IInternationalTransferFeeCalculator international,
        IWithdrawalFeeCalculator withdrawal,
        IDepositFeeCalculator deposit,
        IServiceProvider serviceProvider,
        ILogger<FeeCalculationService> logger,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _domestic = domestic;
        _international = international;
        _withdrawal = withdrawal;
        _deposit = deposit;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;

        var strategyConfig = _configuration["FeeSettings:StackingStrategy"] ?? "Multiplicative";
        _stackingStrategy = Enum.Parse<ModifierStackingStrategy>(strategyConfig);
    }

    public async Task<FeeCalculationResult> CalculateFee(FeeCalculationRequest request, int withdrawalCount)
    {
        try
        {
            // Base fee calculation
            decimal baseFee = CalculateBaseFee(request, withdrawalCount);
            _logger.LogInformation("Base fee: {BaseFee}", baseFee);

            var appliedModifiers = new List<FeeModifierResult>();
            decimal currentFee = baseFee;

            // Get modifiers in priority order
            var orderedModifierTypes = FeeModifierRegistry.GetOrderedModifiers();

            foreach (var modifierConfig in orderedModifierTypes)
            {
                var modifier = _serviceProvider.GetService(modifierConfig.ModifierType) as IFeeModifier;
                if (modifier == null) continue;

                // Apply modifier and get updated fee
                decimal newFee = modifier.ApplyModifier(request, currentFee, out string modifierName);

                if (newFee != currentFee)
                {
                    decimal feeDifference = currentFee - newFee;

                    // Apply multiplicative stacking if applicable
                    if (_stackingStrategy == ModifierStackingStrategy.Multiplicative && appliedModifiers.Any())
                    {
                        decimal percentageChange = feeDifference / currentFee;
                        newFee = currentFee * (1 - percentageChange);
                    }

                    currentFee = Math.Max(newFee, 0);

                    // Use FeeModifierResult DTO instead of entity
                    appliedModifiers.Add(new FeeModifierResult(
                        ModifierName: modifierName,
                        Amount: feeDifference,
                        Priority: modifierConfig.Priority
                    ));

                    _logger.LogInformation("Modifier applied: {ModifierName}, FeeDifference: {FeeDifference}, NewFee: {NewFee}",
                        modifierName, feeDifference, currentFee);
                }
            }

            // Calculate total discount applied
            decimal totalDiscount = appliedModifiers.Where(m => m.Amount > 0).Sum(m => m.Amount);

            // Determine if this is a free transaction
            bool isFreeTransaction = appliedModifiers.Any(m => m.ModifierName == FeeModifierType.FirstTransactionFree.ToString());

            // Apply fee rules
            currentFee = ApplyFeeRules(currentFee, request.Amount, totalDiscount, isFreeTransaction);

            decimal finalFee = Math.Round(currentFee, 2);

            _logger.LogInformation("Final fee: {FinalFee}, Discount: {Discount}", finalFee, totalDiscount);

            // Build breakdown
            string breakdown = BuildBreakdown(baseFee, appliedModifiers, totalDiscount, finalFee);

            // Save audit log
            var auditLog = new FeeAuditLog
            {
                UserId = request.UserId,
                TransactionTypeId = request.TransactionTypeId,
                Amount = request.Amount,
                BaseFee = baseFee,
                TotalDiscount = totalDiscount,
                FinalFee = finalFee,
                Breakdown = breakdown,
                StackingStrategy = _stackingStrategy.ToString(),
                TransactionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.FeeAuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();

            return new FeeCalculationResult(
                BaseFee: baseFee,
                AppliedModifiers: appliedModifiers,
                TotalDiscount: totalDiscount,
                FinalFee: finalFee,
                Breakdown: breakdown,
                StackingStrategy: _stackingStrategy.ToString(),
                CreatedAt: DateTime.UtcNow
            );
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }

    private decimal CalculateBaseFee(FeeCalculationRequest request, int withdrawalCount)
    {
        decimal baseFee = request.TransactionTypeId switch
        {
            (int)TransactionType.Domestic => _domestic.CalculateBaseFee(request.Amount, 0),
            (int)TransactionType.International => _international.CalculateBaseFee(request.Amount, 0),
            (int)TransactionType.Withdrawal => _withdrawal.CalculateBaseFee(request.Amount, request.PaymentMethodId, withdrawalCount),
            (int)TransactionType.Deposit => _deposit.CalculateBaseFee(request.Amount, request.PaymentMethodId),
            _ => throw new ArgumentException("Invalid transaction type")
        };

        return baseFee;
    }

    private decimal ApplyFeeRules(decimal fee, decimal amount, decimal totalDiscount = 0m, bool isFreeTransaction = false)
    {
        // Free transactions should always be zero
        if (isFreeTransaction)
            return 0m;

        // Ensure the total discount never exceeds the original fee
        if (totalDiscount >= fee)
            return 0m;

        // Apply the fee cap: cannot exceed 10% of transaction amount
        decimal maxFee = amount * 0.10m;
        if (fee > maxFee) fee = maxFee;

        // Apply minimum fee for any paid transaction: $0.25
        if (fee > 0 && fee < 0.25m) fee = 0.25m;

        // Ensure fee is never negative
        if (fee < 0m) fee = 0m;

        return fee;
    }

    private string BuildBreakdown(decimal baseFee, List<FeeModifierResult> modifiers, decimal totalDiscount, decimal finalFee)
    {
        // Start with the base fee
        var breakdown = $"BaseFee: {baseFee:C2}";

        // If any modifiers were applied, append their details
        if (modifiers.Any())
        {
            var modifierDetails = string.Join(", ",
                modifiers.Select(m =>
                    $"{m.ModifierName} ({(m.Amount > 0 ? "-" : "+")}{Math.Abs(m.Amount):C2})"));
            breakdown += $" | Modifiers: {modifierDetails}";
        }

        // Add total discount
        breakdown += $" | TotalDiscount: {totalDiscount:C2}";

        // Add the final fee at the end
        breakdown += $" | FinalFee: {finalFee:C2}";

        return breakdown;
    }
}