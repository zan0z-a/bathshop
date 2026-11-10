using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class RepairRequest
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, Phone, StringLength(30)]
    public string Phone { get; set; } = "";

    [EmailAddress, StringLength(150)]
    public string? Email { get; set; }

    public int? ServiceId { get; set; }
    public Service? Service { get; set; }

    [Required, StringLength(1500)]
    public string Description { get; set; } = "";

    [StringLength(40)]
    public string Status { get; set; } = "Новая";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ApplicationUserId { get; set; }
}
