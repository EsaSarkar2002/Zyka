using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public TimeSlotPeriod Period { get; set; }

        [Required]
        [StringLength(20)]
        public string DisplayText { get; set; }

        public bool IsActive { get; set; } = true;
    }
}