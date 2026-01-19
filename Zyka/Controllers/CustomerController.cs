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


        [Authorize(Roles = "Customer")]
        public IActionResult Reservation()
        {
            var timeSlots = _context.TimeSlots.Where(t => t.IsActive).OrderBy(t => t.StartTime).ToList();
            return View(timeSlots);
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Confirmation() { return View(); }

        [Authorize(Roles = "Customer")]
        public IActionResult Payment() { return View(); }


        // APIs

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult GetAvailableTables(DateTime reservationDate, int timeSlotId, TableCategory category)
        {
            var reservedTables = _context.Reservations
            .Where(r => r.ReservationDate == reservationDate.Date &&
            r.TimeSlotId == timeSlotId)
            .Select(r => r.TableId)
            .ToList();
            var availableTables = _context.Tables.Where(t => t.Category == category && !reservedTables.Contains(t.TableId))
            .Select(t => t.TableId)
            .ToList();

            return Ok(availableTables);

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
        public IActionResult CreateSupportTicket([FromBody] SupportTicket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid support data");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ticket.UserId = userId;
            ticket.Status = SupportTicketStatus.Open;
            ticket.CreatedAt = DateTime.UtcNow;

            _context.SupportTickets.Add(ticket);
            _context.SaveChanges();

            return Ok();
        }


        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Support()
        {
            return View();
        }








        //[Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult GetAvailableTimeSlots(DateTime reservationDate, TableCategory category)
        {
            Console.WriteLine($"{reservationDate},{category}");
            var result = new List<TimeSlotAvailabilityDto>();

            var timeSlots = _context.TimeSlots
            .Where(t => t.IsActive)
            .ToList();

            var totalTables = _context.Tables.Count(t => t.Category == category && t.IsActive);

            foreach (var slot in timeSlots)
            {
                var reservedCount = _context.Reservations.Join(_context.Tables,r=>r.TableId,t=>t.TableId,(r,t)=>new {r,t}).Count(x => x.r.ReservationDate == reservationDate.Date && x.r.TimeSlotId == slot.TimeSlotId && x.t.Category == category && x.t.IsActive);
                Console.WriteLine($"{(int)reservedCount},{(int)totalTables}");
                result.Add(new TimeSlotAvailabilityDto
                {
                    TimeSlotId = slot.TimeSlotId,
                    IsAvailable = reservedCount <= totalTables
                });
            }
            return Ok(result);
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
    }
}


