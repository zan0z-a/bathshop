using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class Service
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    [Required, StringLength(250)]
    public string ShortDescription { get; set; } = "";

    [Required, StringLength(1500)]
    public string Description { get; set; } = "";

    [Range(0, 10000000)]
    public decimal PriceFrom { get; set; }
}
