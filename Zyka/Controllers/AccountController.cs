using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Zyka.Models.Entities;

using Zyka.Models.Enums;

using Zyka.ViewModels;

using Newtonsoft.Json;

using System.Net.Http.Json;

namespace Zyka.Controllers

{

    public class AccountController : Controller

    {

        private readonly HttpClient _httpClient;

        // This URL must match your API's actual running address

        private readonly string ApiUrl = "https://localhost:7154/api/AccountApi/Authenticate";

        public AccountController()

        {

            _httpClient = new HttpClient();

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

            // 1. SERIALIZATION: PostAsJsonAsync converts the 'model' to JSON and sends it

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(ApiUrl, model);

            if (response.IsSuccessStatusCode)

            {

                // 2. DESERIALIZATION: Read the JSON response and convert it back to a User object

                var data = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<User>(data);

                if (user != null)

                {

                    // 3. AUTHENTICATION: Create claims from the deserialized user object

                    var claims = new List<Claim>

                    {

                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),

                        new Claim(ClaimTypes.Name, user.UserName),

                        new Claim(ClaimTypes.Email, user.EmailAddress),

                        new Claim(ClaimTypes.Role, user.Role.ToString())

                    };

                    var identity = new ClaimsIdentity(claims, "ZykaCookie");

                    var principal = new ClaimsPrincipal(identity);

                    // Create the session cookie

                    await HttpContext.SignInAsync("ZykaCookie", principal);

                    // 4. ROLE-BASED REDIRECTION: Restored your original logic

                    if (user.Role == UserRole.Admin)

                        return RedirectToAction("Dashboard", "Admin");

                    if (user.Role == UserRole.Customer)

                        return RedirectToAction("Index", "Home");

                    if (user.Role == UserRole.Staff)

                        return RedirectToAction("Dashboard", "Staff");

                    return RedirectToAction("Index", "Home");

                }

            }

            // If API returns an error or user is null

            ModelState.AddModelError("", "Invalid email or password");

            return View(model);

        }

        [Authorize]

        [HttpPost]

        public async Task<IActionResult> Logout()

        {

            await HttpContext.SignOutAsync("ZykaCookie");

            return RedirectToAction("Index", "Home");

        }

        // Note: For a full API integration, the Register method 

        // should also be updated to call an API endpoint similarly to Login.

    }

}
