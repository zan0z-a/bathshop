using BathroomRenovation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BathroomRenovation.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<RepairRequest> RepairRequests => Set<RepairRequest>();
}
