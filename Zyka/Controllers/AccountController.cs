using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zyka.Data;
using Zyka.Models.Entities;
using Zyka.Models.Enums;
using Zyka.Security;
using Zyka.ViewModels;

namespace Zyka.Controllers
{
    public class AccountController : Controller
    {
        // 1. Static HttpClient (Shared across all requests for performance)
        private static readonly HttpClient _httpClient;

        // 2. Instance Database Context (Injected per request)
        private readonly ZykaDbContext _context;

        // --- STATIC CONSTRUCTOR: Sets up HttpClient once ---
        static AccountController()
        {
            var handler = new HttpClientHandler();
            // Bypass SSL certificate issues on localhost
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);
        }

        // --- INSTANCE CONSTRUCTOR: Injects the Database Context ---
        public AccountController(ZykaDbContext context)
        {
            _context = context;
        }

        // Use 127.0.0.1 to avoid DNS resolution issues on localhost
        private readonly string ApiUrl = "https://localhost:7132/api/AccountApi/Authenticate";

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Home");

            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, new
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    options.Converters.Add(new JsonStringEnumConverter());

                    var user = await response.Content.ReadFromJsonAsync<User>(options);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName ?? ""),
                            new Claim(ClaimTypes.Email, user.EmailAddress),
                            new Claim(ClaimTypes.Role, user.Role.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, "ZykaCookie");
                        await HttpContext.SignInAsync("ZykaCookie", new ClaimsPrincipal(identity));

                        return user.Role switch
                        {
                            UserRole.Admin => RedirectToAction("Dashboard", "Admin"),
                            UserRole.Staff => RedirectToAction("Dashboard", "Staff"),
                            _ => RedirectToAction("Index", "Home")
                        };
                    }
                }
                TempData["LoginError"] = "Invalid email or password.";
            }
            catch (Exception ex)
            {
                TempData["LoginError"] = $"Error: {ex.Message} | Inner: {ex.InnerException?.Message}";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // 1. Capture Validation Errors (e.g., Password mismatch)
            if (!ModelState.IsValid)
            {
                // Collect all errors into a single string
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join(" ", errors);

                // 2. Set a flag so the View knows to show the Signup form
                TempData["ShowSignup"] = true;

                return RedirectToAction("Index", "Home");
            }

            // 3. Check for existing user
            bool exists = await _context.Users.AnyAsync(u => u.EmailAddress == model.Email);
            if (exists)
            {
                TempData["ErrorMessage"] = "This email address is already in use.";
                TempData["ShowSignup"] = true; // Keep signup visible
                return RedirectToAction("Index", "Home");
            }

            // 4. Save User
            var newUser = new User
            {
                UserName = model.UserName,
                EmailAddress = model.Email,
                HashedPassword = PasswordHasher.Hash(model.Password),
                Role = UserRole.Customer,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            // Do NOT set ShowSignup here, so it defaults back to Login form
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost] // Changed to Get if you are calling it via a simple link, keep Post if using a form
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("ZykaCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}