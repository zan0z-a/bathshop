using BathroomRenovation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

public class PortfolioController : Controller
{
    private readonly ApplicationDbContext _db;

    public PortfolioController(ApplicationDbContext db)
    {
        _db = db;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index() => View(await _db.PortfolioItems.OrderByDescending(x => x.Year).ToListAsync());
}
