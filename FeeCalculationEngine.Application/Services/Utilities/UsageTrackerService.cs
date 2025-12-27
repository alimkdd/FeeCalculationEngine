using FeeCalculationEngine.Application.Dtos.Requests;
using FeeCalculationEngine.Application.Interfaces.Utilities;
using FeeCalculationEngine.Domain.Models;
using FeeCalculationEngine.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using TransactionType = FeeCalculationEngine.Application.Enums.TransactionType;

namespace FeeCalculationEngine.Application.Services.Utilities;

public class UsageTrackerService : IUsageTrackerService
{
    private readonly AppDbContext _dbContext;

    public UsageTrackerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int GetMonthlyWithdrawalCount(TransactionCountRequest request)
    {
        try
        {
            var monthStart = new DateTime(request.ReferenceDate.Year, request.ReferenceDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            return _dbContext.UserTransactionHistories
                .AsNoTracking()
                .Count(tx => tx.UserId == request.UserId &&
                             tx.TransactionTypeId == (int)TransactionType.Withdrawal &&
                             tx.TransactionDate >= monthStart &&
                             tx.TransactionDate <= monthEnd);
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }

    public void AddTransaction(TransactionRequest request)
    {
        try
        {
            var userTransactionHistory = new UserTransactionHistory
            {
                UserId = request.UserId,
                TransactionTypeId = request.TransactionTypeId,
                TransactionDate = DateTime.UtcNow,
                Amount = request.Amount,
                FeeCharged = request.Fee,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.UserTransactionHistories.Add(userTransactionHistory);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw e.InnerException;
        }
    }
}