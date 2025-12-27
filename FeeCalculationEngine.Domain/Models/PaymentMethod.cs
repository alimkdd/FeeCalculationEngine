using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class PaymentMethod
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}