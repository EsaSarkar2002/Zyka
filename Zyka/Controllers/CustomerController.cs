using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zyka.Data;
using Zyka.Models;
using Zyka.Models.DTOs;
using Zyka.Models.Enums;
using Zyka.Services;
using Zyka.ViewModels;

namespace Zyka.Controllers
{
    public class CustomerController : Controller
    {


        private readonly ZykaDbContext _context;
        private readonly ITableAvailabilityService _tableAvailabilityService;

        public CustomerController(
        ZykaDbContext context,
        ITableAvailabilityService tableAvailabilityService)
        {
            _context = context;
            _tableAvailabilityService = tableAvailabilityService;
        }

        public IActionResult AboutUs() { return View(); }
        public IActionResult Gallery() { return View(); }

        public IActionResult ReservationHistory()

        {

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))

                return Forbid();

            var bookings = _context.Reservations

                .Include(r => r.Table)

                .Include(r => r.TimeSlot)

                .Where(r => r.CustomerId == userId)

                .OrderByDescending(r => r.CreatedAt)

                .Select(r => new BookingHistoryViewModel

                {

                    BookingCode = r.ReservationId.ToString(),

                    CustomerName = r.FullName,

                    Guests = r.NumberOfGuests,

                    TableType = r.Table.Category.ToString(),

                    Status = r.Status.ToString(),

                    Category = r.Table.Category.ToString().ToLower(),

                    // ✅ NEW

                    ReservationDate = r.ReservationDate,

                    TimeSlotText = r.TimeSlot.DisplayText,

                    PhoneNumber = r.MobileNumber

                })

                .ToList();

            return View(bookings);

        }
    


        //[Authorize(Roles = "Customer")]
        //public IActionResult Reservation()
        //{
        //    var timeSlots = _context.TimeSlots
        //        .Where(t => t.IsActive)
        //        .OrderBy(t => t.StartTime)
        //        .ToList();

        //    if (TempData["AvailableSlotIds"] != null)
        //    {
        //        ViewBag.AvailableSlotIds =
        //            System.Text.Json.JsonSerializer
        //                .Deserialize<List<int>>(TempData["AvailableSlotIds"].ToString());
        //    }

        //    return View(timeSlots);
        //}

        [Authorize(Roles = "Customer")]
        public IActionResult Reservation()

        {

            var timeSlots = _context.TimeSlots.Where(t => t.IsActive).OrderBy(t => t.StartTime).ToList();

            return View(timeSlots);

        }

        [Authorize(Roles = "Customer")]
        public IActionResult Confirmation() { 
            return View(); 
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Payment() { return View(); }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Support()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tickets = _context.SupportTickets.Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).ToList();

            var userName = _context.Users.Where(u => u.UserId == userId).Select(u => u.EmailAddress).FirstOrDefault();
            var userEmail = _context.Users.Where(u => u.UserId == userId).Select(u => u.EmailAddress).FirstOrDefault();
            ViewBag.UserEmail = userEmail;
            ViewBag.UserName = userName;

            return View(tickets);
        }

        
        // APIs

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult GetAvailableTimeSlots([FromBody] TimeSlotAvailabilityDto request)
        {
            var selectedDate = request.ReservationDate.Date;

            // 1️⃣ Tables for category
            var tableIds = _context.Tables
                .Where(t =>
                    t.Category == request.Category &&
                    t.IsActive &&
                    t.Status == TableStatus.Available)
                .Select(t => t.TableId)
                .ToList();

            if (!tableIds.Any())
                return Ok(new List<int>());

            // 2️⃣ All active slots
            var slotIds = _context.TimeSlots
                .Where(ts => ts.IsActive)
                .Select(ts => ts.TimeSlotId)
                .ToList();

            // 3️⃣ Booked reservations
            var booked = _context.Reservations
                .Where(r =>
                    r.ReservationDate == selectedDate &&
                    tableIds.Contains(r.TableId) &&
                    r.Status == ReservationStatus.Confirmed)
                .Select(r => new { r.TableId, r.TimeSlotId })
                .ToList();

            // 4️⃣ Availability check
            List<int> availableSlotIds = new();

            foreach (var slotId in slotIds)
            {
                bool hasFreeTable = tableIds.Any(tableId =>
                    !booked.Any(b =>
                        b.TableId == tableId &&
                        b.TimeSlotId == slotId));

                if (hasFreeTable)
                    availableSlotIds.Add(slotId);
            }

            return Ok(availableSlotIds);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult GetAvailableTables(DateTime reservationDate, int timeSlotId, TableCategory category)
        {
            var reservedTables = _context.Reservations
            .Where(r => r.ReservationDate == reservationDate.Date &&
            r.TimeSlotId == timeSlotId && r.Status == ReservationStatus.Confirmed)
            .Select(r => r.TableId)
            .ToList();
            var availableTables = _context.Tables.Where(t => t.Category == category && !reservedTables.Contains(t.TableId) && t.Status == TableStatus.Available)
            .Select(t => t.TableId)
            .ToList();

            return Ok(new
            {
                ReservedTableIds = reservedTables,
                AvailableTableIds = availableTables
            });

        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult CreatePayment(int reservationId, PaymentMethod method)
        {
            var reservation = _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
                return BadRequest("Invalid reservation");

            if (_context.Payments.Any(p => p.ReservationId == reservationId))
                return BadRequest("Payment already completed");

            decimal amount = reservation.Table.Category switch
            {
                TableCategory.Date => 499,
                TableCategory.Family => 999,
                TableCategory.Meeting => 2499,
                TableCategory.Celebration => 4999,
                _ => 400
            };

            _context.Payments.Add(new Payment
            {
                ReservationId = reservationId,
                Amount = amount,
                PaymentMethod = method,
                PaymentStatus = PaymentStatus.Success,
                PaidAt = DateTime.UtcNow
            });

            _context.SaveChanges();
            return Ok();
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult CreateReservation(
        [FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid booking data");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Console.WriteLine(userId);

            var availableTables = _tableAvailabilityService.GetAvailableTables(dto.ReservationDate, dto.TimeSlotId,
            dto.Category);
            var table = availableTables.FirstOrDefault();

            if (table == null)
            {
                return BadRequest("No tables available");
            }

            var reservation = new Reservation
            {
                CustomerId = userId,
                TableId = table.TableId,
                ReservationDate = dto.ReservationDate.Date,
                TimeSlotId = dto.TimeSlotId,
                Status = ReservationStatus.Confirmed,
                NumberOfGuests = dto.NumberOfGuests,
                FullName = dto.FullName,
                MobileNumber = dto.MobileNumber,
                WhatsAppNumber = dto.WhatsAppNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Booking confirmed",
                reservationId = reservation.ReservationId
            });
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult CreateSupportTicket([FromBody] CreateSupportTicketDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid support data");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ticket = new SupportTicket
            {
                UserId = userId,
                CustomerName = dto.CustomerName.Trim(),
                PhoneNumber = dto.PhoneNumber.Trim(),
                Email = dto.Email.Trim(),
                ReservationId = dto.ReservationId,
                Query = dto.Query.Trim(),
                Status = SupportTicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _context.SupportTickets.Add(ticket);
            _context.SaveChanges();

            return Ok(new { ticketId = ticket.TicketId });
        }

        [Authorize(Roles ="Customer")]
        [HttpPost]
        public IActionResult CancelReservation(int reservationId)

        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reservation = _context.Reservations

                .FirstOrDefault(r =>

                    r.ReservationId == reservationId &&

                    r.CustomerId == userId &&

                    r.Status == ReservationStatus.Confirmed);

            if (reservation == null)

                return NotFound("Reservation not found");

            if (reservation.Status == ReservationStatus.Cancelled)

                return BadRequest("Reservation already cancelled");

            if (reservation.Status == ReservationStatus.Completed)

                return BadRequest("Completed reservation cannot be cancelled");

            reservation.Status = ReservationStatus.Cancelled;

            reservation.LastUpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(new { message = "Reservation cancelled successfully" });

        }


    }
}


