using CatalogWebAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyApplicationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Catalog")
        ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "studycatalog.db")}"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<MyApplicationContext>().Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

