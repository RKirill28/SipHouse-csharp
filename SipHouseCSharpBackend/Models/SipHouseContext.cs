using Microsoft.EntityFrameworkCore;

namespace SipHouseCSharpBackend.Models;

public class SipHouseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Image> Images { get; set; }

    // public SipHouseContext()  => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=siphouse.db");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Trace);
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Project>(entity =>
    //     {
    //         
    //     });
    // }
}