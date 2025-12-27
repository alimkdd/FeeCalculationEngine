using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class FeeAuditLog
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }

    [Required]
    public int TransactionTypeId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public decimal BaseFee { get; set; }

    [Required]
    public decimal TotalDiscount { get; set; }

    [Required]
    public decimal FinalFee { get; set; }

    [Required]
    public string Breakdown { get; set; }

    [Required]
    public string StackingStrategy { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public TransactionType TransactionType { get; set; }
}