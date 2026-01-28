using System.ComponentModel.DataAnnotations;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.Data;

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

            // 1. Fetch user from DB

            var user = await _context.Users

                .FirstOrDefaultAsync(u => u.EmailAddress == request.Email && u.IsActive);

            // 2. Verify Password

            if (user == null || !PasswordHasher.Verify(request.Password, user.HashedPassword))

            {

                return Unauthorized("Invalid email or password");

            }

            // 3. Return User object (This is automatically Serialized to JSON)

            return Ok(user);

        }

    }

    public class LoginRequest

    {

        [Required]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }

        [Required]

        [DataType(DataType.Password)]

        public string Password { get; set; }

    }

}
