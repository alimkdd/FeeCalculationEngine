using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class Tier
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public decimal DiscountPercentage { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; }
}