using FeeCalculationEngine.Application.Interfaces.FeeCalculators;

namespace FeeCalculationEngine.Application.Services.FeeCalculation;

public class DomesticTransferFeeCalculator : IDomesticTransferFeeCalculator
{
    public decimal CalculateBaseFee(decimal amount, int paymentMethodId, int? withdrawalCount = null)
    {
        try
        {
            if (amount <= 100) return 1.50m;
            if (amount <= 1000) return amount * 0.015m;
            return (amount * 0.005m) + 15;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}