using System.ComponentModel.DataAnnotations;
using Zyka.Data;
using Zyka.Models.Enums;

namespace Zyka.Models.DTOs
{
    public class CreateReservationDto
    {
        [Required]

        public int TimeSlotId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public TableCategory Category { get; set; }
        [Required]

        public int NumberOfGuests { get; set; }

        [Required]

        public string FullName { get; set; } = string.Empty;

        [Required]

        public string MobileNumber { get; set; } = string.Empty;

        public string? WhatsAppNumber { get; set; }
    }
}


