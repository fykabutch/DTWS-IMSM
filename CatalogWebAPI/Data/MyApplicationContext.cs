using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Data;

public class MyApplicationContext : DbContext
{
    public MyApplicationContext(DbContextOptions<MyApplicationContext> options) : base(options) { }
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialty>()
            .HasOne(x => x.Direction).WithMany().HasForeignKey(x => x.DirectionId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Specialization>()
            .HasOne(x => x.Specialty).WithMany().HasForeignKey(x => x.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

