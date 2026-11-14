using Microsoft.AspNetCore.Mvc;

namespace BathroomRenovation.Controllers;

public class TeamController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Tasks() => View();
}
