using System.ComponentModel.DataAnnotations;

namespace Zyka.ViewModels
{
    public class LoginViewModel
    {
        [Required,EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
