using BathroomRenovation.Models;

namespace BathroomRenovation.ViewModels;

public class AdminDashboardViewModel
{
    public int RequestsCount { get; set; }
    public int NewRequestsCount { get; set; }
    public int UsersCount { get; set; }
    public int ReviewsCount { get; set; }
    public List<RepairRequest> Requests { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
