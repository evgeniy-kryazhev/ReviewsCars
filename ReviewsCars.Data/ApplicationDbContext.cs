using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewsCars.Entities;

namespace ReviewsCars.Data;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<CarImage> ImageCars { get; set; } = null!;
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<CarReview> CarReviews { get; set; } = null!;

    private const string SqlConnectionString = "Data Source=app.db";

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(SqlConnectionString);
    }
}