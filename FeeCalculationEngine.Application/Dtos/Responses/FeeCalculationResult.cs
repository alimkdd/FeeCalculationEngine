namespace FeeCalculationEngine.Application.Dtos.Responses;

public record FeeCalculationResult(
    decimal BaseFee,
    List<FeeModifierResult> AppliedModifiers,
    decimal TotalDiscount,
    decimal FinalFee,
    string Breakdown,
    string StackingStrategy,
    DateTime CreatedAt);