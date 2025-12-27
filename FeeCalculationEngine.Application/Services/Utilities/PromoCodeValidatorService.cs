using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Infrastructure.Context;

namespace FeeCalculationEngine.Application.Services.Utilities;

public class PromoCodeValidatorService : IPromoCodeValidatorService
{
    private readonly AppDbContext _dbContext;

    public PromoCodeValidatorService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public decimal GetDiscountPercentage(int promoCodeId, DateTime transactionDate)
    {
        if (promoCodeId is 0)
            return 0m;

        var code = _dbContext.PromoCodes.FirstOrDefault(pc => pc.Id == promoCodeId && pc.IsActive);

        if (code == null)
            return 0m;

        // Promo code expired
        if (transactionDate > code.ExpiryDate)
            return 0m;

        return code.DiscountPercentage;
    }
}