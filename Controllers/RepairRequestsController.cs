using BathroomRenovation.Data;
using BathroomRenovation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Controllers;

[Authorize]
public class RepairRequestsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RepairRequestsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? serviceId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        await LoadServices(serviceId);

        var model = new RepairRequest
        {
            Name = user.FullName,
            Phone = user.PhoneNumber ?? "",
            Email = user.Email,
            ServiceId = serviceId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RepairRequest model)
    {
        if (model.ServiceId.HasValue && !await _db.Services.AnyAsync(x => x.Id == model.ServiceId.Value))
            ModelState.AddModelError(nameof(model.ServiceId), "Выберите услугу из списка.");

        if (!ModelState.IsValid)
        {
            await LoadServices(model.ServiceId);
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var request = new RepairRequest
        {
            Name = model.Name.Trim(),
            Phone = model.Phone.Trim(),
            Email = model.Email?.Trim(),
            ServiceId = model.ServiceId,
            Description = model.Description.Trim(),
            ApplicationUserId = user.Id,
            Status = "Новая",
            CreatedAt = DateTime.UtcNow
        };
        _db.RepairRequests.Add(request);
        await _db.SaveChangesAsync();
        return View("Success", request);
    }

    private async Task LoadServices(int? selectedId = null)
    {
        ViewBag.Services = new SelectList(await _db.Services.OrderBy(x => x.Title).ToListAsync(), "Id", "Title", selectedId);
    }
}
