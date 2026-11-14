using BathroomRenovation.Models;

namespace BathroomRenovation.ViewModels;

public class CabinetViewModel
{
    public ApplicationUser User { get; set; } = new();
    public List<RepairRequest> Requests { get; set; } = new();
}
