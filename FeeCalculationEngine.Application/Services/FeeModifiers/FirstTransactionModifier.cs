using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Interfaces.Utilities;

namespace FeeCalculationEngine.Application.Services.FeeModifiers;

public class FirstTransactionModifier : IFeeModifier
{
    private readonly ITierService _tierService;

    public FirstTransactionModifier(ITierService tierService)
    {
        _tierService = tierService;
    }
    public decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName)
    {
        modifierName = FeeModifierType.FirstTransactionFree.ToString();

        if (_tierService.IsFirstTransaction(request.UserId))
        {
            return 0m;
        }

        return currentFee;
    }
}