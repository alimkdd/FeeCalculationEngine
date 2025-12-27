namespace FeeCalculationEngine.Application.Interfaces.Utilities;

public interface IPromoCodeValidatorService
{
    decimal GetDiscountPercentage(int promoCodeId, DateTime transactionDate);
}