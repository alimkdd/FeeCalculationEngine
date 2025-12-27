namespace FeeCalculationEngine.Application.Dtos.Responses;

public record FeeModifierResult(
    string ModifierName,
    decimal Amount,
    int Priority
);