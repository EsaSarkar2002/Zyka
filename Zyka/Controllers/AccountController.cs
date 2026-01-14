using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zyka.Data;
using Zyka.Models;
using Zyka.Models.Enums;
using Zyka.Security;
using Zyka.ViewModels;

namespace Zyka.Controllers
{
    public class AccountController : Controller
    {
        private readonly ZykaDbContext _context;
        public AccountController(ZykaDbContext context)
        {
            _context = context;
        }
        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users
                .FirstOrDefault(u => u.EmailAddress == model.Email && u.IsActive);

            if (user == null || !PasswordHasher.Verify(model.Password, user.HashedPassword))
            {
                return BadRequest("Invalid email or password");
            }

            // AUTHENTICATION
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "ZykaCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("ZykaCookie", principal);

            if (user.Role == UserRole.Admin)
                return RedirectToAction("Dashboard", "Admin");

            if (user.Role == UserRole.Customer)
                return RedirectToAction("Index", "Home");

            if (user.Role == UserRole.Staff)
                return RedirectToAction("Dashboard", "Staff");

            // Fallback: if role is not recognized, redirect to home
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("ZykaCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExists = _context.Users.Any(u => u.EmailAddress == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("EmailAddress", "Email already registered");
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                EmailAddress = model.Email,
                HashedPassword = PasswordHasher.Hash(model.Password),
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // AUTHENTICATION
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "ZykaCookie");
            var principal = new ClaimsPrincipal(identity);

            //Create the Cookie
            await HttpContext.SignInAsync("ZykaCookie", principal);

            return RedirectToAction("Index", "Home");
        }
    }
}
