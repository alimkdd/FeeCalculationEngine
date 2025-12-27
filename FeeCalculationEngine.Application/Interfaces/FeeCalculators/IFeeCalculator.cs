namespace FeeCalculationEngine.Application.Interfaces.FeeCalculators;

public interface IFeeCalculator
{
    decimal CalculateBaseFee(decimal amount, int paymentMethodId, int? withdrawalCount = null);
}