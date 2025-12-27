using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Enums;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FeeCalculationEngine.Application.Services.Utilities;

public class TierService : ITierService
{
    private readonly AppDbContext _dbContext;

    public TierService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsPremium(int userId)
    {
        try
        {
            var user = _dbContext.Users.Include(u => u.Tier).FirstOrDefault(u => u.Id == userId);

            return user.Tier.Name == Tier.Premium.ToString();
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }

    public bool IsHighVolume(TransactionCountRequest request)
    {
        try
        {
            // Use the provided ReferenceDate, or default to UtcNow if it's default
            var referenceDate = request.ReferenceDate != default ? request.ReferenceDate : DateTime.UtcNow;

            // Get the first and last date of the month
            var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            // Count transactions in the month
            int count = _dbContext.UserTransactionHistories
                .Count(tx => tx.UserId == request.UserId && tx.TransactionDate >= monthStart && tx.TransactionDate <= monthEnd);

            return count > 50;
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }

    public bool IsFirstTransaction(int userId)
    {
        try
        {
            return !_dbContext.UserTransactionHistories.Any(tx => tx.UserId == userId);
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}