using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        //User/Customer table foreign key
        [Required]
        public int CustomerId { get; set; }
        public User Customer { get; set; }

        //TableInfo table foreign key
        [Required]
        public int TableId { get; set; }
        public TableInfo Table { get; set; }

        //Date and TimeSlot for reservation
        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }

        //Reservation details
        [Required]
        public ReservationStatus Status { get; set; }

        [Required,Range(1,20)]
        public int NumberOfGuests { get; set; }

        //Customer contacts
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(15)]
        public string MobileNumber { get; set; }

        [StringLength(15)]
        public string? WhatsAppNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
    }
}