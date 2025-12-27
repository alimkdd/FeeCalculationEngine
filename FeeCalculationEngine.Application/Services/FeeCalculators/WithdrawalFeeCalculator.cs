using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeCalculators;

namespace FeeCalculationEngine.Application.Services.FeeCalculators;

public class WithdrawalFeeCalculator : IWithdrawalFeeCalculator
{
    public decimal CalculateBaseFee(decimal amount, int paymentMethodId, int? withdrawalCount = null)
    {
        try
        {
            withdrawalCount ??= 0;

            decimal fee = withdrawalCount <= 3 ? 0 : 2.50m;

            if (paymentMethodId == (int)PaymentMethod.ATM)
                fee += 1.50m;

            return fee;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}