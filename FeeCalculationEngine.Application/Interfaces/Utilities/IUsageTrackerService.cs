using FeeCalculationEngine.Application.Dtos.Requests;

namespace FeeCalculationEngine.Application.Interfaces.Utilities;

public interface IUsageTrackerService
{
    int GetMonthlyWithdrawalCount(TransactionCountRequest request);

    void AddTransaction(TransactionRequest request);
}