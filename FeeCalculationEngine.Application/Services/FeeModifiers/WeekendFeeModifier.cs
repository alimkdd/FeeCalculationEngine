using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.FeeModification;
using FeeCalculationEngine.Application.Interfaces.Utilities;

namespace FeeCalculationEngine.Application.Services.FeeModification;

public class WeekendFeeModifier : IFeeModifier
{
    private readonly IBusinessCalendarService _calendar;

    public WeekendFeeModifier(IBusinessCalendarService calendar)
    {
        _calendar = calendar;
    }

    public decimal ApplyModifier(FeeCalculationRequest request, decimal currentFee, out string modifierName)
    {
        modifierName = FeeModifierType.WeekendOrHolidayFee.ToString();

        if (_calendar.IsWeekendOrHoliday(DateTime.UtcNow))
        {
            return currentFee + 1m;
        }

        return currentFee;
    }
}