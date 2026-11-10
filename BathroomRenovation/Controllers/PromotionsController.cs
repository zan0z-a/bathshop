using BathroomRenovation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

public class PromotionsController : Controller
{
    private readonly ApplicationDbContext _db;

    public PromotionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index() => View(await _db.Promotions.ToListAsync());
}
