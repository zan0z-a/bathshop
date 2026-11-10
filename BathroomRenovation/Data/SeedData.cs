using BathroomRenovation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Data;

public static class SeedData
{
    public static async Task InitializeAsync(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in new[] { "Admin", "Manager", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!await db.Services.AnyAsync())
        {
            db.Services.AddRange(
                new Service
                {
                    Title = "Реставрация ванны",
                    ShortDescription = "Обновим покрытие ванны без полной замены.",
                    Description = "Восстанавливаем поверхность ванн, устраняем мелкие дефекты " +
                        "и возвращаем аккуратный внешний вид.",
                    PriceFrom = 8500
                },
                new Service
                {
                    Title = "Ремонт ванной под ключ",
                    ShortDescription = "Полный ремонт с понятными этапами и сметой.",
                    Description = "Помогаем спланировать ремонт, подобрать материалы и выполнить " +
                        "отделочные работы в одном проекте.",
                    PriceFrom = 65000
                },
                new Service
                {
                    Title = "Замена сантехники",
                    ShortDescription = "Установка раковин, смесителей и душевых систем.",
                    Description = "Демонтируем старое оборудование и устанавливаем новую " +
                        "сантехнику с проверкой соединений.",
                    PriceFrom = 5500
                },
                new Service
                {
                    Title = "Обновление плитки",
                    ShortDescription = "Локальная замена и укладка плитки.",
                    Description = "Подготовим основание, заменим повреждённые элементы или " +
                        "выполним новую укладку плитки.",
                    PriceFrom = 12000
                }
            );
        }

        if (!await db.PortfolioItems.AnyAsync())
        {
            db.PortfolioItems.AddRange(
                new PortfolioItem
                {
                    Title = "Светлая ванная",
                    Description = "Обновление отделки и установка новой сантехники.",
                    ImageUrl = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?auto=format&fit=crop&w=1000&q=85",
                    Year = 2026
                },
                new PortfolioItem
                {
                    Title = "Современная душевая",
                    Description = "Душевая зона со стеклянной перегородкой и новой плиткой.",
                    ImageUrl = "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?auto=format&fit=crop&w=1000&q=85",
                    Year = 2025
                },
                new PortfolioItem
                {
                    Title = "Ванная в спокойных тонах",
                    Description = "Реставрация и аккуратное обновление интерьера.",
                    ImageUrl = "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?auto=format&fit=crop&w=1000&q=85",
                    Year = 2025
                }
            );
        }

        if (!await db.Promotions.AnyAsync())
        {
            db.Promotions.AddRange(
                new Promotion
                {
                    Title = "Бесплатный замер",
                    Description = "При заказе ремонта ванной выезд специалиста и первичный замер — бесплатно.",
                    ExpirationText = "До конца месяца"
                },
                new Promotion
                {
                    Title = "Скидка на реставрацию",
                    Description = "Скидка 10% на реставрацию ванны при заказе через сайт.",
                    ExpirationText = "Количество предложений ограничено"
                }
            );
        }

        if (!await db.Reviews.AnyAsync())
        {
            db.Reviews.AddRange(
                new Review
                {
                    Author = "Анна",
                    Text = "Ванна выглядит как новая. Работу выполнили аккуратно и в срок.",
                    Rating = 5,
                    IsPublished = true
                },
                new Review
                {
                    Author = "Михаил",
                    Text = "Понравилось, что заранее объяснили этапы и стоимость работ.",
                    Rating = 5,
                    IsPublished = true
                }
            );
        }

        await db.SaveChangesAsync();

        var adminEmail = "admin@gmail.com";
        var adminPassword = "Admin12345";

        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Администратор"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            else if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
