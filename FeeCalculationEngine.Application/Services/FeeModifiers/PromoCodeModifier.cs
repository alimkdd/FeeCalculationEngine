using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Interfaces.Utilities;

namespace FeeCalculationEngine.Application.Services.FeeModifiers;

public class PromoCodeModifier : IFeeModifier
{
    private readonly IPromoCodeValidatorService _promoValidator;

    public PromoCodeModifier(IPromoCodeValidatorService promoValidator)
    {
        _promoValidator = promoValidator;
    }

    public decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName)
    {
        modifierName = FeeModifierType.PromoCodeDiscount.ToString();

        if (request.PromoCodeId == 0)
            return currentFee;

        var discountPercent = _promoValidator.GetDiscountPercentage(request.PromoCodeId, DateTime.UtcNow);

        if (discountPercent <= 0)
            return currentFee;

        decimal discount = currentFee * (discountPercent / 100);
        return currentFee - discount;
    }
}