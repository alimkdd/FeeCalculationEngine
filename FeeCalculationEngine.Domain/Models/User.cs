namespace FeeCalculationEngine.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int TierId { get; set; }
    public Tier? Tier { get; set; }
    public bool IsHighVolume { get; set; } = false;
    public bool IsFirstTime { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}