using CatalogClientApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogClientApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("http://localhost:5024/api/Account", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Не удалось войти. Проверьте логин и пароль.");
                return View(model);
            }
            var user = await response.Content.ReadFromJsonAsync<LoginResult>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user!.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var identity = new ClaimsIdentity(claims, scheme);
            var properties = new AuthenticationProperties
            {
                ExpiresUtc = new DateTimeOffset(user.ExpiresUtc)
            };
            properties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = user.Token }
            });
            await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity), properties);
            return RedirectToAction("Index", "Specialties");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("http://localhost:5024/api/Account/Register", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Регистрация не выполнена. Проверьте данные или выберите другой логин.");
                return View(model);
            }
            TempData["Message"] = "Регистрация завершена. Теперь войдите.";
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}

