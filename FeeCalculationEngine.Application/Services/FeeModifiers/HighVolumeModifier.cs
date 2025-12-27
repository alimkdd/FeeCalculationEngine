using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Interfaces.Utilities;

namespace FeeCalculationEngine.Application.Services.FeeModifiers;

public class HighVolumeModifier : IFeeModifier
{
    private readonly ITierService _tierService;

    public HighVolumeModifier(ITierService tierService)
    {
        _tierService = tierService;
    }

    public decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName)
    {
        modifierName = FeeModifierType.HighVolumeDiscount.ToString();

        if (_tierService.IsHighVolume(new TransactionCountRequest(request.UserId, DateTime.UtcNow)))
        {
            decimal discount = currentFee * 0.10m;
            return currentFee - discount;
        }

        return currentFee;
    }
}
