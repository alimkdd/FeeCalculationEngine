using System.ComponentModel.DataAnnotations;

namespace FeeCalculationEngine.Domain.Models;

public class BusinessHoliday
{
    public int Id { get; set; }

    [Required]
    public DateTime HolidaysDate { get; set; }

    [Required]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}