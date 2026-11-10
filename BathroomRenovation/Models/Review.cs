using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class Review
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Author { get; set; } = "";

    [Required, StringLength(1000)]
    public string Text { get; set; } = "";

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public string? UserId { get; set; }
}
