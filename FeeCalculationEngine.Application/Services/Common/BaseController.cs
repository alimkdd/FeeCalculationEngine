using FeeCalculationEngine.Application.Interfaces;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FeeCalculationEngine.Application.Services.Common;

public class BaseController : ControllerBase
{
    public readonly IFeesService _feeService;
    public readonly IFeeCalculationService _feeCalculationService;
    public readonly IUsageTrackerService _usageTracker;
    public readonly ITierService _tierService;
    public readonly IBusinessCalendarService _calendar;
    public readonly IPromoCodeValidatorService _promoValidator;
    public BaseController(
        IFeesService feeService,
        IFeeCalculationService feeCalculationService,
        IUsageTrackerService usageTracker,
        ITierService tierService,
        IBusinessCalendarService calendar,
        IPromoCodeValidatorService promoValidator)
    {
        _feeService = feeService;
        _feeCalculationService = feeCalculationService;
        _usageTracker = usageTracker;
        _tierService = tierService;
        _calendar = calendar;
        _promoValidator = promoValidator;
    }
}