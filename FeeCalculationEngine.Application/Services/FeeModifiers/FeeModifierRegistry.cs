using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Services.FeeModification;

namespace FeeCalculationEngine.Application.Services.FeeModifiers;

public class FeeModifierRegistry
{
    public static List<FeeModifierConfiguration> GetOrderedModifiers() => _modifierConfigurations.OrderBy(m => m.Priority).ToList();

    private static readonly List<FeeModifierConfiguration> _modifierConfigurations = new()
    {
        // Priority 1 (highest) - Binary modifiers that eliminate fees
        new(1, ModifierStackingStrategy.Additive, typeof(FirstTransactionModifier)),
    
        // Priority 2 - User tier benefits
        new(2, ModifierStackingStrategy.Additive, typeof(PremiumUserModifier)),
    
        // Priority 3 - Volume-based discounts
        new(3, ModifierStackingStrategy.Additive, typeof(HighVolumeModifier)),
    
        // Priority 4 - Promotional discounts
        new(4, ModifierStackingStrategy.Additive, typeof(PromoCodeModifier)),
    
        // Priority 5 (lowest) - Weekend Fees
        new(5, ModifierStackingStrategy.Additive, typeof(WeekendFeeModifier))
    };
}