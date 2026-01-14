using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zyka.Models.DTOs;
using Zyka.Models.Enums;
namespace Zyka.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles ="Admin")]
        public IActionResult Dashboard()
        {
            return View(); // by view() ASP.NET will find a view whose name is same as the action name(i.e, Dashboard here). Like it'll search for Dashboard.cshtml
        }
        public IActionResult Bookings()
        {
            ViewBag.Bookings = GetBookings();
            return View();
        }
        public IActionResult History()
        {
            ViewBag.Bookings = GetBookings();
            return View();
        }

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

        private List<object> GetBookings()
        {
            var today = DateTime.Today;

            return new List<object>
    {
        new {
            CustomerName = "John Smith",
            TableCategory = "date",
            TableNumber = "D-02",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Sarah Johnson",
            TableCategory = "meeting",
            TableNumber = "M-02",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "18:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Michael Chen",
            TableCategory = "meeting",
            TableNumber = "M-03",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "cancelled"
        },
        new {
            CustomerName = "Emma Wilson",
            TableCategory = "celebration",
            TableNumber = "C-03",
            Date = new DateTime(2025, 12, 15).ToString("yyyy-MM-dd"),
            Time = "19:30",
            Status = "completed"
        },
        new {
            CustomerName = "David Brown",
            TableCategory = "date",
            TableNumber = "D-01",
            Date = new DateTime(2025, 12, 16).ToString("yyyy-MM-dd"),
            Time = "18:00",
            Status = "completed"
        },
        new {
            CustomerName = "Olivia Davis",
            TableCategory = "family",
            TableNumber = "F-01",
            Date = new DateTime(2025, 12, 20).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "completed"
        },
        new {
            CustomerName = "James Miller",
            TableCategory = "date",
            TableNumber = "D-03",
            Date = new DateTime(2026, 1, 5).ToString("yyyy-MM-dd"),
            Time = "19:30",
            Status = "completed"
        },
        new {
            CustomerName = "Sophia Taylor",
            TableCategory = "meeting",
            TableNumber = "M-01",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "21:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Daniel Anderson",
            TableCategory = "meeting",
            TableNumber = "M-04",
            Date = new DateTime(2026, 1, 7).ToString("yyyy-MM-dd"),
            Time = "20:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Isabella Martinez",
            TableCategory = "celebration",
            TableNumber = "C-01",
            Date = new DateTime(2026, 1, 8).ToString("yyyy-MM-dd"),
            Time = "18:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "William Garcia",
            TableCategory = "family",
            TableNumber = "F-02",
            Date = new DateTime(2026, 1, 2).ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "cancelled"
        },
        new {
            CustomerName = "Mia Rodriguez",
            TableCategory = "date",
            TableNumber = "D-04",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "17:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Ethan Martinez",
            TableCategory = "meeting",
            TableNumber = "M-05",
            Date = new DateTime(2026, 1, 10).ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Charlotte Lee",
            TableCategory = "meeting",
            TableNumber = "M-02",
            Date = new DateTime(2026, 1, 12).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Henry Walker",
            TableCategory = "celebration",
            TableNumber = "C-02",
            Date = new DateTime(2026, 1, 15).ToString("yyyy-MM-dd"),
            Time = "18:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Amelia Hall",
            TableCategory = "family",
            TableNumber = "F-03",
            Date = new DateTime(2025, 12, 28).ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "completed"
        },
        new {
            CustomerName = "Lucas Allen",
            TableCategory = "date",
            TableNumber = "D-05",
            Date = today.ToString("yyyy-MM-dd"),
            Time = "20:15",
            Status = "confirmed"
        },
        new {
            CustomerName = "Harper Young",
            TableCategory = "meeting",
            TableNumber = "M-03",
            Date = new DateTime(2026, 1, 18).ToString("yyyy-MM-dd"),
            Time = "18:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Benjamin King",
            TableCategory = "meeting",
            TableNumber = "M-04",
            Date = new DateTime(2026, 1, 20).ToString("yyyy-MM-dd"),
            Time = "19:30",
            Status = "cancelled"
        },
        new {
            CustomerName = "Ella Scott",
            TableCategory = "celebration",
            TableNumber = "C-03",
            Date = new DateTime(2026, 1, 22).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Alexander Green",
            TableCategory = "family",
            TableNumber = "F-04",
            Date = new DateTime(2026, 1, 25).ToString("yyyy-MM-dd"),
            Time = "18:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Grace Adams",
            TableCategory = "date",
            TableNumber = "D-01",
            Date = new DateTime(2026, 1, 26).ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Jack Nelson",
            TableCategory = "meeting",
            TableNumber = "M-05",
            Date = new DateTime(2026, 1, 28).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Chloe Carter",
            TableCategory = "meeting",
            TableNumber = "M-01",
            Date = new DateTime(2026, 1, 30).ToString("yyyy-MM-dd"),
            Time = "21:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Matthew Perez",
            TableCategory = "celebration",
            TableNumber = "C-01",
            Date = new DateTime(2026, 2, 1).ToString("yyyy-MM-dd"),
            Time = "19:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Victoria Rivera",
            TableCategory = "family",
            TableNumber = "F-05",
            Date = new DateTime(2026, 2, 3).ToString("yyyy-MM-dd"),
            Time = "18:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Samuel Brooks",
            TableCategory = "date",
            TableNumber = "D-02",
            Date = new DateTime(2026, 2, 5).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Scarlett Murphy",
            TableCategory = "meeting",
            TableNumber = "M-02",
            Date = new DateTime(2026, 2, 7).ToString("yyyy-MM-dd"),
            Time = "19:30",
            Status = "confirmed"
        },
        new {
            CustomerName = "Joseph Bailey",
            TableCategory = "meeting",
            TableNumber = "M-03",
            Date = new DateTime(2026, 2, 10).ToString("yyyy-MM-dd"),
            Time = "20:00",
            Status = "confirmed"
        },
        new {
            CustomerName = "Lily Cooper",
            TableCategory = "celebration",
            TableNumber = "C-02",
            Date = new DateTime(2026, 2, 12).ToString("yyyy-MM-dd"),
            Time = "18:30",
            Status = "confirmed"
        }
    };
        }
    }
}