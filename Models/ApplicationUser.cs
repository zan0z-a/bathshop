using Microsoft.AspNetCore.Identity;

namespace BathroomRenovation.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
}
