using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Dtos.Responses;
using FeeCalculationEngine.Application.Interfaces;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Application.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace FeeCalculationEngine.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class FeesController : BaseController
{
    public FeesController(IFeesService feeService,
                          IFeeCalculationService feeCalculationService,
                          IUsageTrackerService usageTracker,
                          ITierService tierService,
                          IBusinessCalendarService calendar,
                          IPromoCodeValidatorService promoValidator) : base(feeService, feeCalculationService, usageTracker, tierService, calendar, promoValidator) { }

    #region GET

    [HttpGet("Preview")]
    public async Task<FeeCalculationResult> Preview([FromQuery] FeeCalculationRequest request)
         => await _feeService.PreviewFee(request);

    #endregion

    #region POST

    [HttpPost("Calculate")]
    public async Task<ActionResult<FeeCalculationResult>> Calculate([FromBody] FeeCalculationRequest request)
        => await _feeService.CalculateFee(request);

    #endregion
}