using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => //CookieAuthenticationOptions
{
    options.LoginPath = new PathString("/Account/Login");
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "Catalog.Auth";
    options.SlidingExpiration = false;
});
builder.Services.AddAuthorization();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Specialties}/{action=Index}/{id?}");
app.Run();




