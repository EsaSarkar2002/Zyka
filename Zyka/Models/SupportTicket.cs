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
        public int? ReservationId { get; set; }   // booking id
        public Reservation? Reservation { get; set; }

        [Required]
        [StringLength(15),Phone]
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