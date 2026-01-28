using Zyka.Models.Entities;
using Zyka.Models.Enums;

namespace Zyka.Services
{
    public interface ITableAvailabilityService
    {
        List<TableInfo> GetAvailableTables(

            DateTime reservationDate,

            int timeSlotId,

            TableCategory tableCategory
        );
    }
}
