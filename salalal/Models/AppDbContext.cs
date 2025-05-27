using Microsoft.EntityFrameworkCore;

using salalal.Models;

public class AppDbContext : DbContext
{
    //Konstruktor koji prima opcije i prosledjuje ih baznoj klasi
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //DbSet svojstva koja predstavljaju tabele u bazi
    public DbSet<User> Users { get; set; }
    public DbSet<Ski> Skis { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   //Povezuje OrderItem sa Ski pomoću foreign key-a (SkiId)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Ski)
            .WithMany()
            .HasForeignKey(oi => oi.SkiId);

        // Slika moze biti null(bug bez ovoga?)
        modelBuilder.Entity<Ski>()
            .Property(s => s.ImagePath)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}
