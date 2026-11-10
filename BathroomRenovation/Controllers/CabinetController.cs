using BathroomRenovation.Data;
using BathroomRenovation.Models;
using BathroomRenovation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

[Authorize]
public class CabinetController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CabinetController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var model = new CabinetViewModel
        {
            User = user,
            Requests = await _db.RepairRequests.Include(x => x.Service)
                .Where(x => x.ApplicationUserId == user.Id)
                .OrderByDescending(x => x.CreatedAt).ToListAsync()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(string fullName, string? phone)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 100 || (phone?.Length ?? 0) > 30)
        {
            TempData["Message"] = "Проверьте введённые данные.";
            return RedirectToAction(nameof(Index));
        }

        user.FullName = fullName.Trim();
        user.PhoneNumber = phone?.Trim();
        await _userManager.UpdateAsync(user);
        TempData["Message"] = "Профиль сохранён.";
        return RedirectToAction(nameof(Index));
    }
}
