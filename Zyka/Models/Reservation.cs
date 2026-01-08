using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zyka.Models
{
    public enum BookingType
    {
        Dating,   // 2 seats
        Family,   // 4-6 seats
        Meeting,  // 8-12 seats
        Party     // 15-20 seats
    }

    //[Table("Reservations")]
    public class Reservations
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("TableInfo")]
        public int TableId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [StringLength(20)]
        public string TimeSlot { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(10)]
        public string MobileNumber { get; set; }

        [StringLength(10)]
        public string WhatsAppNumber { get; set; }

        // ✅ New property for Booking Type
        [Required]
        public BookingType BookingType { get; set; }

        // ✅ Optional: Seats info based on booking type
        [NotMapped] // Not stored in DB, derived from BookingType
        public string SeatRange
        {
            get
            {
                return BookingType switch
                {
                    BookingType.Dating => "2 seats",
                    BookingType.Family => "4-6 seats",
                    BookingType.Meeting => "8-12 seats",
                    BookingType.Party => "15-20 seats",
                    _ => "N/A"
                };
            }
        }
    }
}