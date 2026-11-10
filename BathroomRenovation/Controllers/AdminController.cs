using BathroomRenovation.Data;
using BathroomRenovation.Models;
using BathroomRenovation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            RequestsCount = await _db.RepairRequests.CountAsync(),
            NewRequestsCount = await _db.RepairRequests.CountAsync(x => x.Status == "Новая"),
            UsersCount = await _userManager.Users.CountAsync(),
            ReviewsCount = await _db.Reviews.CountAsync(),
            Requests = await _db.RepairRequests.Include(x => x.Service).OrderByDescending(x => x.CreatedAt).Take(30).ToListAsync(),
            Reviews = await _db.Reviews.OrderByDescending(x => x.CreatedAt).Take(30).ToListAsync()
        };
        ViewBag.IsAdmin = User.IsInRole("Admin");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRequestStatus(int id, string status)
    {
        var allowed = new[] { "Новая", "В работе", "Завершена", "Отменена" };
        if (!allowed.Contains(status))
            return BadRequest();
        var request = await _db.RepairRequests.FindAsync(id);
        if (request == null)
            return NotFound();
        request.Status = status;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request = await _db.RepairRequests.FindAsync(id);
        if (request == null)
            return NotFound();
        _db.RepairRequests.Remove(request);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review == null)
            return NotFound();
        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Services() => View(await _db.Services.OrderBy(x => x.Id).ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveService(Service model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Проверьте поля услуги.";
            return RedirectToAction(nameof(Services));
        }
        if (model.Id == 0)
            _db.Services.Add(model);
        else
        {
            var item = await _db.Services.FindAsync(model.Id);
            if (item == null)
                return NotFound();
            item.Title = model.Title;
            item.ShortDescription = model.ShortDescription;
            item.Description = model.Description;
            item.PriceFrom = model.PriceFrom;
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Services));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        var item = await _db.Services.FindAsync(id);
        if (item != null)
        {
            var requests = await _db.RepairRequests.Where(x => x.ServiceId == id).ToListAsync();
            foreach (var request in requests)
                request.ServiceId = null;
            _db.Services.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Services));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Portfolio() => View(await _db.PortfolioItems.OrderByDescending(x => x.Year).ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePortfolioItem(PortfolioItem model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Проверьте поля проекта и ссылку на фото.";
            return RedirectToAction(nameof(Portfolio));
        }
        if (model.Id == 0)
            _db.PortfolioItems.Add(model);
        else
        {
            var item = await _db.PortfolioItems.FindAsync(model.Id);
            if (item == null)
                return NotFound();
            item.Title = model.Title;
            item.Description = model.Description;
            item.ImageUrl = model.ImageUrl;
            item.Year = model.Year;
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Portfolio));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePortfolioItem(int id)
    {
        var item = await _db.PortfolioItems.FindAsync(id);
        if (item != null)
        {
            _db.PortfolioItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Portfolio));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Promotions() => View(await _db.Promotions.OrderBy(x => x.Id).ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePromotion(Promotion model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Проверьте поля акции.";
            return RedirectToAction(nameof(Promotions));
        }
        if (model.Id == 0)
            _db.Promotions.Add(model);
        else
        {
            var item = await _db.Promotions.FindAsync(model.Id);
            if (item == null)
                return NotFound();
            item.Title = model.Title;
            item.Description = model.Description;
            item.ExpirationText = model.ExpirationText;
        }
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Promotions));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePromotion(int id)
    {
        var item = await _db.Promotions.FindAsync(id);
        if (item != null)
        {
            _db.Promotions.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Promotions));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.OrderBy(x => x.Email).ToListAsync();
        var model = new List<AdminUserViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new AdminUserViewModel { User = user, Role = roles.FirstOrDefault() ?? "User" });
        }
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(string fullName, string email, string password, string role)
    {
        if (!new[] { "User", "Manager", "Admin" }.Contains(role) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || password.Length < 8)
        {
            TempData["Message"] = "Проверьте данные нового пользователя. Пароль — от 8 символов.";
            return RedirectToAction(nameof(Users));
        }
        var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName.Trim(), EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, role);
        else
            TempData["Message"] = string.Join(" ", result.Errors.Select(x => x.Description));
        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(string id, string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 100 || !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
        {
            TempData["Message"] = "Проверьте имя и email пользователя.";
            return RedirectToAction(nameof(Users));
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        var nameResult = await _userManager.SetUserNameAsync(user, email);
        var emailResult = await _userManager.SetEmailAsync(user, email);
        if (!nameResult.Succeeded || !emailResult.Succeeded)
        {
            TempData["Message"] = string.Join(" ", nameResult.Errors.Concat(emailResult.Errors).Select(x => x.Description));
            return RedirectToAction(nameof(Users));
        }
        user.FullName = fullName.Trim();
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(string id, string role)
    {
        if (!new[] { "User", "Manager", "Admin" }.Contains(role))
            return BadRequest();
        if (id == _userManager.GetUserId(User) && role != "Admin")
            return BadRequest();
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (id == _userManager.GetUserId(User))
            return BadRequest();
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
            await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(Users));
    }
}
