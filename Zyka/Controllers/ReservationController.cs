using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Zyka.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ReservationController: Controller
    {
        public IActionResult Index()
        { 
            return View("~/Views/CustomerController/ReservationController/Index().cshtnl");
        }
        public IActionResult Confirmation()
        {
            return View("~/Views/CustomerController/ReservationController/Confirmation().cshtnl");
        }

        public IActionResult Payment()
        {
            return View("~/Views/CustomerController/ReservationController/Payment().cshtnl");
        }
    }
}
