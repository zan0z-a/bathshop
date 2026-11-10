using System.ComponentModel.DataAnnotations;

namespace BathroomRenovation.Models;

public class PortfolioItem
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    [Required, StringLength(500)]
    public string Description { get; set; } = "";

    [Required, Url, StringLength(500)]
    public string ImageUrl { get; set; } = "";

    [Range(2000, 2100)]
    public int Year { get; set; } = DateTime.Now.Year;
}
