using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Infrastructure.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FeeCalculationEngine.Application.Services.Utilities;

public class BusinessCalendar : IBusinessCalendarService
{
    private readonly AppDbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;
    private string HolidaysCacheKey;

    public BusinessCalendar(AppDbContext dbContext, IConfiguration configuration, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _cache = cache;

        HolidaysCacheKey = _configuration["HolidaysCacheKey"];
    }

    public bool IsWeekendOrHoliday(DateTime date)
    {
        // Check weekend first
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        // Load holidays from cache
        var holidaysJson = _cache.GetStringAsync(HolidaysCacheKey).GetAwaiter().GetResult();
        HashSet<DateTime> holidays;

        if (string.IsNullOrEmpty(holidaysJson))
        {
            holidays = LoadHolidaysToCache();
        }
        else
        {
            holidays = JsonSerializer.Deserialize<HashSet<DateTime>>(holidaysJson);
        }

        return holidays.Contains(date.Date);
    }

    private HashSet<DateTime> LoadHolidaysToCache()
    {
        var holidays = _dbContext.BusinessHolidays
                                 .Select(h => h.HolidaysDate.Date)
                                 .ToHashSet();

        var json = JsonSerializer.Serialize(holidays);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };

        _cache.SetStringAsync(HolidaysCacheKey, json, options).GetAwaiter().GetResult();
        return holidays;
    }
}