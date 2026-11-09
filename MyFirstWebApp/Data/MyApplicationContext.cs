using Microsoft.EntityFrameworkCore;

namespace MyFirstWebApp.Data;

public class MyApplicationContext : DbContext
{
    public MyApplicationContext(DbContextOptions<MyApplicationContext> options) : base(options)
    {
    }

    public DbSet<Specialty> Specialties { get; set; }
}
