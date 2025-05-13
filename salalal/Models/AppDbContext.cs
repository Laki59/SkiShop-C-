using Microsoft.EntityFrameworkCore;

using salalal.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Ski> Skis { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Ski)
            .WithMany()
            .HasForeignKey(oi => oi.SkiId);

        // Slika moze biti null i ima max karaktera(bug bez ovoga?)
        modelBuilder.Entity<Ski>()
            .Property(s => s.ImagePath)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}
