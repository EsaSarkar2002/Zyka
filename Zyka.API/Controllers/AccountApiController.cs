using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zyka.API.Data;
using Zyka.API.Security;

namespace Zyka.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly ZykaDbContext _context;

        public AccountApiController(ZykaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Invalid request.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress == request.Email && u.IsActive);

            if (user == null || !PasswordHasher.Verify(request.Password, user.HashedPassword))
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(user);
        }
    }

    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}