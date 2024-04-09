using DemoBlazorServerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoBlazorServerApp;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Example configuration
        modelBuilder.Entity<User>().HasKey(u => u.Id);
    }
}

