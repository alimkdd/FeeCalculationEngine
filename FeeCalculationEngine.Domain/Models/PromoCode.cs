using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class PromoCode
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; }

    public decimal DiscountPercentage { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}