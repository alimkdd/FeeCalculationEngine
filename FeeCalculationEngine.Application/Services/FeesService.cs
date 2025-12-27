using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Dtos.Responses;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using Microsoft.Extensions.Logging;

namespace FeeCalculationEngine.Application.Services;

public class FeesService : IFeesService
{
    private readonly IFeeCalculationService _feeCalculationService;
    private readonly IUsageTrackerService _usageTracker;
    private readonly ILogger<FeesService> _logger;

    public FeesService(
        IFeeCalculationService feeCalculationService,
        IUsageTrackerService usageTracker,
        ILogger<FeesService> logger)
    {
        _feeCalculationService = feeCalculationService;
        _usageTracker = usageTracker;
        _logger = logger;
    }

    public async Task<FeeCalculationResult> PreviewFee(FeeCalculationRequest request) => await ProcessFeeCalculation(request, persist: false);

    public async Task<FeeCalculationResult> CalculateFee(FeeCalculationRequest request) => await ProcessFeeCalculation(request, persist: true);

    private async Task<FeeCalculationResult> ProcessFeeCalculation(FeeCalculationRequest request, bool persist)
    {
        try
        {
            int withdrawalCount = 0;

            // Calculate withdrawal count if necessary
            if (request.TransactionTypeId == (int)TransactionType.Withdrawal)
            {
                var countRequest = new TransactionCountRequest(request.UserId, DateTime.UtcNow);
                withdrawalCount = _usageTracker.GetMonthlyWithdrawalCount(countRequest) + 1;

                _logger.LogInformation("Withdrawal count for UserId {UserId}: {WithdrawalCount}", request.UserId, withdrawalCount);
            }

            // Calculate fee, passing withdrawalCount as a separate parameter
            var result = await _feeCalculationService.CalculateFee(request, withdrawalCount);

            if (result == null)
                return null;

            // Persist only if real calculation
            if (persist)
            {
                var transaction = new TransactionRequest(
                    UserId: request.UserId,
                    TransactionTypeId: request.TransactionTypeId,
                    Amount: request.Amount,
                    Fee: result.FinalFee
                );

                _usageTracker.AddTransaction(transaction);
                _logger.LogInformation("Transaction persisted for UserId {UserId}: {Transaction}", request.UserId, transaction);
            }

            return result;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}