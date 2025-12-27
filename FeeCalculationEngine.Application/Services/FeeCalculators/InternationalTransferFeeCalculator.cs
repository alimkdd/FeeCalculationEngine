using FeeCalculationEngine.Application.Interfaces.FeeCalculators;

namespace FeeCalculationEngine.Application.Services.FeeCalculation;

public class InternationalTransferFeeCalculator : IInternationalTransferFeeCalculator
{
    public decimal CalculateBaseFee(decimal amount, int paymentMethodId, int? withdrawalCount = null)
    {
        try
        {
            decimal fee = (amount * 0.03m) + 5;
            if (amount > 5000) fee += 2;
            return fee;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}