using BathroomRenovation.Data;
using BathroomRenovation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var reviews = await _db.Reviews
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return View(reviews);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string text, int rating)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > 1000 || rating < 1 || rating > 5)
        {
            TempData["Message"] = "Проверьте текст отзыва и оценку.";
            return RedirectToAction(nameof(Index));
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var review = new Review
        {
            Author = user.FullName,
            Text = text.Trim(),
            Rating = rating,
            UserId = user.Id,
            IsPublished = true
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Спасибо! Ваш отзыв опубликован.";
        return RedirectToAction(nameof(Index));
    }
}
