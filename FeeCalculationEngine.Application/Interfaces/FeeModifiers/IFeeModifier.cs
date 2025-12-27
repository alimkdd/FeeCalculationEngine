using FeeCalculationEngine.Application.Dtos.Requests;

namespace FeeCalculationEngine.Application.Interfaces.FeeModification;

public interface IFeeModifier
{
    decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName);
}