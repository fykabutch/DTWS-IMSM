using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Data;

public class MyApplicationContext : DbContext
{
    public DbSet<Specialty> Specialties { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=studycatalog.db");
        }
    }


}
