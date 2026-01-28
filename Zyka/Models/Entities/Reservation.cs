using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Zyka.Models.Enums;

namespace Zyka.Models.Entities

{
    [Index(nameof(TableId), nameof(ReservationDate), nameof(TimeSlotId), IsUnique = true)]
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        //User/Customer table foreign key
        [Required]
        public int CustomerId { get; set; }
        [Required, ForeignKey(nameof(CustomerId))]
        public User Customer { get; set; }

        //TableInfo table foreign key
        [Required]
        public int TableId { get; set; }
        [Required]
        public TableInfo Table { get; set; }

        //Date and TimeSlot for reservation
        [Required, Column(TypeName = "date")]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int TimeSlotId { get; set; }
        [Required]
        public TimeSlot TimeSlot { get; set; }

        //Reservation details
        [Required]
        public ReservationStatus Status { get; set; }

        public Payment Payment { get; set; }

        [Required, Range(1, 20)]
        public int NumberOfGuests { get; set; }

        //Customer contacts
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, Phone, StringLength(15)]
        public string MobileNumber { get; set; }

        [StringLength(15)]
        public string? WhatsAppNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
        public int UserId { get; internal set; }
    }
}