using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyApplicationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Catalog")
        ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "studycatalog.db")}"));
builder.Services.AddControllers();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Введите JWT-токен без слова Bearer."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // требуется валидация издателя
            ValidIssuer = JwtAuthOptions.Issuer, // издатель
            ValidateAudience = true, // требуется валидация потребителя
            ValidAudience = JwtAuthOptions.Audience, // потребитель
            ValidateLifetime = true, // требуется валидация времени жизни
            IssuerSigningKey = JwtAuthOptions.SecurityKey, // ключ безопасности
            ValidateIssuerSigningKey = true, // валидация ключа безопасности
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyApplicationContext>();
    context.Database.Migrate();
    if (!context.Users.Any(x => x.NormalizedUsername == "ADMIN"))
    {
        var admin = new User { Username = "admin", NormalizedUsername = "ADMIN", Role = "Admin" };
        var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<User>>();
        admin.PasswordHash = hasher.HashPassword(admin, "admin");
        context.Users.Add(admin);
        context.SaveChanges();
    }
}
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();



