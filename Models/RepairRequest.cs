using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class RepairRequest
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, Phone, RegularExpression(@"^(?:\+7|7|8)[\s\-]*\(?[3-9][0-9]{2}\)?[\s\-]*[0-9]{3}[\s\-]*[0-9]{2}[\s\-]*[0-9]{2}$", ErrorMessage = "Введите корректный российский номер: 8 (900) 000-00-00."), StringLength(18)]
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
