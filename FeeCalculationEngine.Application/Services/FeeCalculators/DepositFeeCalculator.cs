using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeCalculators;

namespace FeeCalculationEngine.Application.Services.FeeCalculation;

public class DepositFeeCalculator : IDepositFeeCalculator
{
    public decimal CalculateBaseFee(decimal amount, int paymentMethodId, int? withdrawalCount = null)
    {
        try
        {
            if (paymentMethodId == (int)PaymentMethod.Card)
            {
                decimal fee = amount * 0.025m;
                if (fee < 0.50m) fee = 0.50m;
                if (fee > 25m) fee = 25m;
                return fee;
            }
            return 0;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}