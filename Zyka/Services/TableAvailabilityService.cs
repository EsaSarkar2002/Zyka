using Zyka.Data;

using Zyka.Models.Entities;
using Zyka.Models.Enums;
namespace Zyka.Services
{
    public class TableAvailabilityService : ITableAvailabilityService
    {
        private readonly ZykaDbContext _context;
        public TableAvailabilityService(ZykaDbContext context)
        {
            _context = context;
        }

        public List<TableInfo> GetAvailableTables(
            DateTime reservationDate,
            int timeSlotId,
            TableCategory category)
        {
            var reservedTableIds = _context.Reservations
            .Where(r =>
            r.ReservationDate == reservationDate &&

            r.TimeSlotId == timeSlotId)

            .Select(r => r.TableId)

            .ToList();

            return _context.Tables

            .Where(t =>
            t.IsActive && t.Category == category && t.Status== TableStatus.Available &&
            !reservedTableIds.Contains(t.TableId)).ToList();

        }

    }

}