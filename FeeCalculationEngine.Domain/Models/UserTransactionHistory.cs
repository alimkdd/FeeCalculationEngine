namespace FeeCalculationEngine.Domain.Models;

public class UserTransactionHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TransactionTypeId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public decimal FeeCharged { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TransactionType TransactionType { get; set; }
}