using BathroomRenovation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

public class ServicesController : Controller
{
    private readonly ApplicationDbContext _db;

    public ServicesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index() => View(await _db.Services.OrderBy(x => x.Id).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var service = await _db.Services.FindAsync(id);
        return service == null ? NotFound() : View(service);
    }
}
