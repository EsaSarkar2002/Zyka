using System.ComponentModel.DataAnnotations;

namespace Zyka.Models.DTOs
{
    public class TimeSlotAvailabilityDto
    {
        [Required]
        public int TimeSlotId { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
    }
}
