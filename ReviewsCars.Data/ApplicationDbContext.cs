using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewsCars.Entities;

namespace ReviewsCars.Data;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<PostCar> PostCars { get; set; } = null!;
    
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