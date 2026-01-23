using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zyka.Data;
using Zyka.Models;
using Zyka.Models.Enums;

namespace Zyka.Controllers
{
    public class StaffController : Controller
    {
        private readonly ZykaDbContext _context;

        public StaffController(ZykaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalTables = await _context.Tables.CountAsync();
            ViewBag.TotalTables = totalTables;

            var today = DateTime.Today;

            var todaysReservations = await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.TimeSlot)
                .Where(r => r.ReservationDate.Date == today)
                .ToListAsync();

            var occupiedTableCount = todaysReservations
                .Where(r => r.Status == ReservationStatus.Confirmed)
                .Select(r => r.TableId)
                .Distinct()
                .Count();

            ViewBag.AvailableTables = totalTables - occupiedTableCount;
            ViewBag.TodayBookingsCount = todaysReservations.Count;
            ViewBag.GuestsToday = todaysReservations.Sum(r => r.NumberOfGuests);

            return View(todaysReservations);
        }

        public async Task<IActionResult> Table(DateTime? filterDate)
        {
            var selectedDate = filterDate ?? DateTime.Today;
            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");

            var tables = await _context.Tables
                .Include(t => t.Reservations)
                .ToListAsync();

            foreach (var table in tables)
            {
                var hasActiveBooking = table.Reservations
                    .Any(r => r.ReservationDate.Date == selectedDate.Date &&
                              r.Status != ReservationStatus.Cancelled &&
                              r.Status != ReservationStatus.Completed);

                table.Status = hasActiveBooking ? TableStatus.Reserved : TableStatus.Available;
            }

            return View(tables);
        }

        public async Task<IActionResult> Booking(DateTime? filterDate, ReservationStatus? filterStatus)
        {
            var query = _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.TimeSlot)
                .AsQueryable();

            if (filterDate.HasValue)
                query = query.Where(r => r.ReservationDate.Date == filterDate.Value.Date);

            if (filterStatus.HasValue)
                query = query.Where(r => r.Status == filterStatus.Value);

            var reservations = await query.OrderByDescending(r => r.ReservationDate).ToListAsync();

            ViewBag.SelectedDate = filterDate?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = filterStatus;

            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.Completed;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Booking));
        }

        // --- DYNAMIC SUPPORT ACTIONS ---

        public async Task<IActionResult> Support()
        {
            var tickets = await _context.SupportTickets
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyTicket(int ticketId, string replyMessage)
        {
            var ticket = await _context.SupportTickets.FindAsync(ticketId);
            if (ticket != null && !string.IsNullOrEmpty(replyMessage))
            {
                ticket.StafReply = replyMessage;
                ticket.Status = SupportTicketStatus.Resolved; // Clears CS0117 error
                ticket.RepliedAt = DateTime.Now;
                ticket.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Support));
        }

        [HttpPost]
        public async Task<IActionResult> MarkResolved(int id)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket != null)
            {
                ticket.Status = SupportTicketStatus.Resolved;
                ticket.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Support));
        }
    }
}