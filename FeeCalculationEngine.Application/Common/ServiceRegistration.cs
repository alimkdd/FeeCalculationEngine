using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Interfaces;
using FeeCalculationEngine.Application.Interfaces.FeeCalculators;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Application.Services;
using FeeCalculationEngine.Application.Services.FeeCalculation;
using FeeCalculationEngine.Application.Services.FeeCalculators;
using FeeCalculationEngine.Application.Services.FeeModification;
using FeeCalculationEngine.Application.Services.FeeModifiers;
using FeeCalculationEngine.Application.Services.Utilities;
using FeeCalculationEngine.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeeCalculationEngine.Application.Common;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        // Fee Main Service
        services.AddScoped<IFeesService, FeesService>();

        // Fee Calculators Services
        services.AddScoped<IDepositFeeCalculator, DepositFeeCalculator>();
        services.AddScoped<IDomesticTransferFeeCalculator, DomesticTransferFeeCalculator>();
        services.AddScoped<IInternationalTransferFeeCalculator, InternationalTransferFeeCalculator>();
        services.AddScoped<IWithdrawalFeeCalculator, WithdrawalFeeCalculator>();

        //Fee Calculation Service
        services.AddScoped<IFeeCalculationService, FeeCalculationService>();

        // Fee Modifiers Services
        services.AddScoped<FirstTransactionModifier>();
        services.AddScoped<PremiumUserModifier>();
        services.AddScoped<HighVolumeModifier>();
        services.AddScoped<PromoCodeModifier>();
        services.AddScoped<WeekendFeeModifier>();

        // Fee Modifier Registry
        services.AddSingleton<FeeModifierRegistry>();

        //Helper / Supporting Services
        services.AddScoped<ITierService, TierService>();
        services.AddScoped<IPromoCodeValidatorService, PromoCodeValidatorService>();
        services.AddScoped<IBusinessCalendarService, BusinessCalendar>();
        services.AddScoped<IUsageTrackerService, UsageTrackerService>();

        // Fluent Validation
        services.AddTransient<IValidator<FeeCalculationRequest>, FeeCalculationRequestValidator>();

        return services;
    }
}