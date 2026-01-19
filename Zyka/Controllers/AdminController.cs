using Zyka.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zyka.Models.DTOs;
using Zyka.Models.Enums;
namespace Zyka.Controllers
{
    public class AdminController : Controller
    {
        private readonly ZykaDbContext _context;

        public AdminController(ZykaDbContext context)

        {

            _context = context;

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



        ///-------------------------/////

        [Authorize(Roles ="Admin")]
        public IActionResult Dashboard()
        {
            return View(); // by view() ASP.NET will find a view whose name is same as the action name(i.e, Dashboard here). Like it'll search for Dashboard.cshtml
        }
        //public IActionResult Bookings()
        //{
        //    ViewBag.Bookings = GetBookings();
        //    return View();
        //}
        //public IActionResult History()
        //{
        //    ViewBag.Bookings = GetBookings();
        //    return View();
        //}

        public IActionResult TableCategories()
        {
            return View();
        }
        public IActionResult TableList(TableCategory category)
        {
            ViewBag.Category = category;

            // dummy data for now
            var tables = new Dictionary<string, string>();

            if (category == TableCategory.Family)
            {
                tables.Add("F-01", "Available");
                tables.Add("F-02", "Booked");
                tables.Add("F-03", "Maintenance");
                tables.Add("F-04", "Available");
                tables.Add("F-05", "Available");
                tables.Add("F-06", "Booked");
            }
            else if (category == TableCategory.Date)
            {
                tables.Add("D-01", "Available");
                tables.Add("D-02", "Booked");
                tables.Add("D-03", "Available");
                tables.Add("D-04", "Available");
                tables.Add("D-05", "Booked");
            }
            else if (category == TableCategory.Meeting)
            {
                tables.Add("M-01", "Available");
                tables.Add("M-02", "Booked");
                tables.Add("M-03", "Available");
                tables.Add("M-04", "Maintenance");
                tables.Add("M-05", "Available");
            }
            else if (category == TableCategory.Celebration)
            {
                tables.Add("C-01", "Booked");
                tables.Add("C-02", "Available");
                tables.Add("C-03", "Maintenance");
                tables.Add("C-04", "Maintenance");
                tables.Add("C-05", "Available");
            }

            return View(tables);
        }
        //public IActionResult TodayBookings()
        //{
        //    ViewData["ActiveMenu"] = "Bookings";
        //    var today = DateTime.Today;

        //    var bookings = GetBookings()
        //    .Where(b => b.Date.Date == today)
        //    .ToList();

        //    return View("Bookings", bookings);
        //}

        //public IActionResult FutureBookings()
        //{
        //    ViewData["ActiveMenu"] = "Bookings";
        //    var today = DateTime.Today;

        //    var bookings = GetBookings()
        //    .Where(b => b.Date > today)
        //    .ToList();

        //    return View("Bookings", bookings);
        //}

        //private List<object> GetBookings()
        //{
        //    var today = DateTime.Today;

        //    return;
        //}
    }
}