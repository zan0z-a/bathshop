using BathroomRenovation.Models;

namespace BathroomRenovation.ViewModels;

public class HomeViewModel
{
    public List<Service> Services { get; set; } = new();
    public List<PortfolioItem> Portfolio { get; set; } = new();
    public List<Promotion> Promotions { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
