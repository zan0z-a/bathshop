using BathroomRenovation.Data;
using BathroomRenovation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            Services = await _db.Services.OrderBy(x => x.Id).Take(4).ToListAsync(),
            Portfolio = await _db.PortfolioItems.OrderByDescending(x => x.Year).Take(3).ToListAsync(),
            Promotions = await _db.Promotions.Take(2).ToListAsync(),
            Reviews = await _db.Reviews.OrderByDescending(x => x.CreatedAt).Take(3).ToListAsync()
        };
        return View(model);
    }

    public IActionResult About() => View();

    public IActionResult Privacy() => View();

    public IActionResult Error() => View();
}
