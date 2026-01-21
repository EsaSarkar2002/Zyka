using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models.DTOs
{
    public class TimeSlotAvailabilityDto
    {        
        public DateTime ReservationDate { get; set; }       
        public TableCategory Category { get; set; }
    }
}
