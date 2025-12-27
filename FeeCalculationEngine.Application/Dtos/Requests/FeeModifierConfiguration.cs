using FeeCalculationEngine.Application.Enums;

namespace FeeCalculationEngine.Application.Dtos.Requests;

public record FeeModifierConfiguration(
    int Priority,
    ModifierStackingStrategy StackingStrategy,
    Type ModifierType);