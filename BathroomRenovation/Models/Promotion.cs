using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class Promotion
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    [Required, StringLength(500)]
    public string Description { get; set; } = "";

    [StringLength(100)]
    public string ExpirationText { get; set; } = "";
}
