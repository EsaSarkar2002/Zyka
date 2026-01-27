using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Zyka.Data;
using Zyka.Data;
using Zyka.Models;
using Zyka.Models.DTOs;
using Zyka.Models.Enums;
using Zyka.Security;
using Zyka.ViewModels;
namespace Zyka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ZykaDbContext _context;

        public AdminController(ZykaDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()

        {
            ViewBag.TotalTables = _context.Tables.Count(t => t.IsActive);
            ViewBag.TadaysReservations = _context.Reservations
            .Where(r => r.ReservationDate == DateTime.Today)
            .Count();
            ViewBag.UpcomingReservations     = _context.Reservations
            .Where(r => r.ReservationDate > DateTime.Today)
            .Count();
            ViewBag.TotalCustomers = _context.Users.Where(u => u.Role == Models.Enums.UserRole.Customer).Count();
            ViewBag.TotalStaffs = _context.Users.Where(u => u.Role == Models.Enums.UserRole.Staff).Count();
            ViewBag.TotalRevenue = _context.Payments.Sum(p => p.Amount);
            ViewBag.MaintenanceTablse = _context.Tables.Where(t => t.Status == Models.Enums.TableStatus.Maintenance).Count();
            return View(); // by view() ASP.NET will find a view whose name is same as the action name(i.e, Dashboard here). Like it'll search for Dashboard.cshtml
        }
        public IActionResult Bookings()

        {
            var bookings =
            (

            from r in _context.Reservations

            join t in _context.Tables on r.TableId equals t.TableId

            join ts in _context.TimeSlots on r.TimeSlotId equals ts.TimeSlotId

            select new BookingsDto

            {

                ReservationId = r.ReservationId.ToString(),

                CustomerName = r.FullName,

                TableCategory = t.Category.ToString(),

                TableNumber = t.TableNumber,

                Date = r.ReservationDate,

                Time = ts.DisplayText,

                Status = r.Status.ToString().ToLower()

            }

            )

            .OrderByDescending(b => b.Date)

            .ToList();

            ViewBag.Bookings = bookings;

            return View();

        }
        public IActionResult TableCategories()
        {
            return View();
        }
        public IActionResult TableList(
    TableCategory category,
    DateTime? date,
    int? timeSlotId)
        {
            DateTime selectedDate = date ?? DateTime.Today;
            Console.WriteLine(timeSlotId);
            ViewBag.Category = category;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.SelectedTimeSlotId = timeSlotId;
            ViewBag.TimeSlots = _context.TimeSlots.ToList();

            var tables = _context.Tables
                .Where(t => t.Category == category && t.IsActive)
                .ToList();

            var bookings = new List<TableAvailabilityViewModel>();

            foreach (var table in tables)
            {
                bool isBooked = false;

                if (timeSlotId != null)
                {
                    isBooked = _context.Reservations.Any(r =>
                        r.TableId == table.TableId &&
                        r.ReservationDate == selectedDate &&
                        r.TimeSlotId == timeSlotId &&
                        r.Status == ReservationStatus.Confirmed
                    );
                }

                bookings.Add(new TableAvailabilityViewModel
                {
                    Table = table,
                    IsBooked = isBooked
                });
            }

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> AddTable(string tableNumber, TableCategory category)
        {
            

            if (_context.Tables.Any(t=>t.TableNumber==tableNumber))
            {
                TempData["Error"] = "Table Already Exists";
            }
            var table = new TableInfo
            {
                TableNumber = tableNumber,
                Category = category,
                Status = TableStatus.Available,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            return RedirectToAction("TableList", new { category });
        }
        //[HttpPost]
        //public async Task<IActionResult> AddStaff(string tableNumber, TableCategory category)
        //{


        //    if (_context.Users.Any(t => t.EmailAddress == tableNumber))
        //    {
        //        TempData["Error"] = "Staff Already Exists. Please use new Email Address.";
        //    }
        //    var table = new User
        //    {
        //        TableNumber = tableNumber,
        //        Category = category,
        //        Status = TableStatus.Available,
        //        IsActive = true,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _context.Tables.Add(table);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("TableList", new { category });
        //}
        [HttpGet]
        public IActionResult IsTableNumberExists(string tableNumber)
        {
            if (string.IsNullOrWhiteSpace(tableNumber))
                return Json(false);

            bool exists = _context.Tables
                .Any(t => t.TableNumber == tableNumber);

            return Json(exists);
        }

        public IActionResult MarkMaintenance(int tableId, TableCategory category)
        {
            var table = _context.Tables.Find(tableId);
            if (table == null) return NotFound();

            table.Status = TableStatus.Maintenance;
            _context.SaveChanges();

            return RedirectToAction("TableList", new { category });
        }
        public IActionResult MarkAvailable(int tableId, TableCategory category)
        {
            var table = _context.Tables.Find(tableId);
            if (table == null) return NotFound();

            table.Status = TableStatus.Available;
            _context.SaveChanges();

            return RedirectToAction("TableList", new { category });
        }

        public IActionResult History()

        {

            var bookings = (

                from r in _context.Reservations

                join t in _context.Tables on r.TableId equals t.TableId

                join ts in _context.TimeSlots on r.TimeSlotId equals ts.TimeSlotId

                select new BookingsDto

                {

                    ReservationId = r.ReservationId.ToString(),

                    CustomerName = r.FullName,

                    TableCategory = t.Category.ToString(),

                    TableNumber = t.TableNumber,

                    Date = r.ReservationDate,

                    Time = ts.DisplayText,

                    Status = r.Status.ToString().ToLower()

                }

            )

            .OrderByDescending(b => b.Date)

            .ToList();

            ViewBag.Bookings = bookings;

            return View();

        }
        //============Staff details===============
        public IActionResult StaffDetails()

        {
            var staff = _context.Users
                .Where(u => u.Role == UserRole.Staff)
                .Select(u => new StaffDto
                {
                    StaffId = u.UserId,
                    Name = u.UserName,
                    Email=u.EmailAddress,
                    IsActive = u.IsActive
                })

                .ToList();

            ViewBag.Staff = staff;

            return View();

        }

        // ================= CUSTOMER DETAILS =================

        [Authorize(Roles = "Admin")]
        public IActionResult CustomerDetails()

        {

            var customers = _context.Users

                .Where(u => u.Role == UserRole.Customer)

                .Select(u => new CustomerListDto

                {

                    UserId = u.UserId,

                    Name = u.UserName,

                    Email = u.EmailAddress,

                    IsActive = u.IsActive,

                    MobileNumber = _context.Reservations

                        .Where(r => r.CustomerId == u.UserId)

                        .OrderByDescending(r => r.ReservationDate)

                        .Select(r => r.MobileNumber)

                        .FirstOrDefault()

                })

                .ToList();

            ViewBag.Customers = customers;

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> AddStaff(AddStaffViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("StaffDetails");

            var staff = new User
            {
                UserName = model.Name,
                EmailAddress = model.Email,
                HashedPassword = PasswordHasher.Hash($"{model.Password}"),
                Role=UserRole.Staff,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(staff);
            await _context.SaveChangesAsync();


            return RedirectToAction("StaffDetails");
        }

        [HttpPost]
        public IActionResult ToggleUserStatus(int userId)

        {

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;

            _context.SaveChanges();

            // ✅ Redirect back to the same page

            return Redirect(Request.Headers["Referer"].ToString());

        }





        //    public IActionResult TableList(
        //TableCategory category,
        //DateTime? date,
        //int? timeSlotId)
        //    {
        //        ViewBag.Category = category;
        //        ViewBag.SelectedDate = date ?? DateTime.Today;
        //        ViewBag.SelectedTimeSlotId = timeSlotId;

        //        var tables = _context.Tables
        //            .Where(t => t.Category == category && t.IsActive)
        //            .ToList();

        //        return View(tables);
        //    }
    }
}