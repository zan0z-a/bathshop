using BathroomRenovation.Models;

namespace BathroomRenovation.ViewModels;

public class AdminUserViewModel
{
    public ApplicationUser User { get; set; } = new();
    public string Role { get; set; } = "User";
}
