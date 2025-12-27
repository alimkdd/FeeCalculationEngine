using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class FeeModifier
{
    public int Id { get; set; }

    [Required]
    public string ModifierName { get; set; }

    [Required]
    public int Priority { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}