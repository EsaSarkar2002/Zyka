using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models.DTOs
{
    public class TableAvailabilityRequestDto

    {

        [Required]

        public DateTime ReservationDate { get; set; }

        [Required]

        public int TimeSlotId { get; set; }

        [Required]
        public TableCategory TableCategory { get; set; }

    }
}
