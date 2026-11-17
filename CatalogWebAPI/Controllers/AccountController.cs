using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly MyApplicationContext _context;
        private readonly PasswordHasher<User> _hasher;

        public AccountController(MyApplicationContext context, PasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            var username = model.Username.Trim();
            if (username.Length < 3)
                return BadRequest("Логин должен содержать не менее 3 символов.");

            var normalized = username.ToUpperInvariant();
            if (_context.Users.Any(x => x.NormalizedUsername == normalized))
                return BadRequest("Этот логин уже занят.");

            var user = new User
            {
                Username = username,
                NormalizedUsername = normalized,
                Role = "User"
            };
            user.PasswordHash = _hasher.HashPassword(user, model.Password);
            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Не удалось зарегистрировать пользователя. Возможно, логин уже занят.");
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<LoginResult> Login(LoginViewModel model)
        {
            var normalized = model.Username.Trim().ToUpperInvariant();
            var user = _context.Users.FirstOrDefault(x => x.NormalizedUsername == normalized);
            if (user == null) return Unauthorized("Неверный логин или пароль.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Неверный логин или пароль.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var expires = DateTime.UtcNow.AddMinutes(JwtAuthOptions.LifeTime);
            var jwt = new JwtSecurityToken(
                issuer: JwtAuthOptions.Issuer,
                audience: JwtAuthOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: new SigningCredentials(JwtAuthOptions.SecurityKey, SecurityAlgorithms.HmacSha256));

            return new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Username = user.Username,
                Role = user.Role,
                ExpiresUtc = expires
            };
        }
    }
}

