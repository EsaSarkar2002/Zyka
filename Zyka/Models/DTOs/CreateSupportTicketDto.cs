using System.ComponentModel.DataAnnotations;

namespace Zyka.Models.DTOs
{
    public class CreateSupportTicketDto
    {
        
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public int? ReservationId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Query { get; set; }
    }
}

