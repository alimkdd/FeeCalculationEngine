using FeeCalculationEngine.Application.Dtos.Requests;

namespace FeeCalculationEngine.Application.Interfaces.Utilities;

public interface ITierService
{
    bool IsPremium(int userId);
    bool IsHighVolume(TransactionCountRequest request);
    bool IsFirstTransaction(int userId);
}