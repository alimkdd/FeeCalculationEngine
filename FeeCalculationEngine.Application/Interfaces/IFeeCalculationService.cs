using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Dtos.Responses;

namespace FeeCalculationEngine.Application.Interfaces;

public interface IFeeCalculationService
{
    Task<FeeCalculationResult> CalculateFee(FeeCalculationRequest request, int withdrawalCount);
}