namespace FeeCalculationEngine.Application.Interfaces.Utilities;

public interface IBusinessCalendarService
{
    bool IsWeekendOrHoliday(DateTime date);
}