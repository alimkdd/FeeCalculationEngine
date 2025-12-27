using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Interfaces.Utilities;

namespace FeeCalculationEngine.Application.Services.FeeModification;

public class PremiumUserModifier : IFeeModifier
{
    private readonly ITierService _tierService;

    public PremiumUserModifier(ITierService tierService)
    {
        _tierService = tierService;
    }

    public decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName)
    {
        try
        {
            modifierName = FeeModifierType.PremiumUserDiscount.ToString();

            if (_tierService.IsPremium(request.UserId))
            {
                decimal discount = currentFee * 0.25m;
                return currentFee - discount;
            }

            return currentFee;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}