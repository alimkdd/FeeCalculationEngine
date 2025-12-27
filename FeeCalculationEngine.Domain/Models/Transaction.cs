namespace FeeCalculationEngine.Domain.Models;

public class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public int TransactionTypeId { get; set; }

    public int PaymentMethodId { get; set; }

    public int PromoCodeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
    public TransactionType TransactionType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PromoCode PromoCode { get; set; }
}