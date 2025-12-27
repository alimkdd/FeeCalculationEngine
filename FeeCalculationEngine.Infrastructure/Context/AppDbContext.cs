using FeeCalculationEngine.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FeeCalculationEngine.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<BusinessHoliday> BusinessHolidays { get; set; }
    public DbSet<FeeAuditLog> FeeAuditLogs { get; set; }
    public DbSet<FeeModifier> FeeModifiers { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Tier> Tiers { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserTransactionHistory> UserTransactionHistories { get; set; }
}