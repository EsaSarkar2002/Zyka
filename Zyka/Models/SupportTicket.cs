using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models
{
    public class SupportTicket
    {
        [Key]
        public int TicketId { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(50)]
        public string BookingReference { get; set; }   // booking id

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string Query { get; set; }

        [Required]
        public SupportTicketStatus Status { get; set; } = SupportTicketStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}