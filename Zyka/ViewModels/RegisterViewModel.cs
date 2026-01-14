using System.ComponentModel.DataAnnotations;

namespace Zyka.ViewModels
{
    public class RegisterViewModel
    {
        [Required,StringLength(50)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8),DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password"),DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
