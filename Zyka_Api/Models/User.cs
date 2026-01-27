using System.ComponentModel.DataAnnotations;

namespace Zyka_Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255)]
        public string HashedPassword { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")] //Validation for email format
        [StringLength(150)]
        public string EmailAddress { get; set; }

        //[Required]
        //public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
