using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Dtos.Responses;

namespace FeeCalculationEngine.Application.Interfaces;

public interface IFeesService
{
    Task<FeeCalculationResult> PreviewFee(FeeCalculationRequest request);

    Task<FeeCalculationResult> CalculateFee(FeeCalculationRequest request);
}