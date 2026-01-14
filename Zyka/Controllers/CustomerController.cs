using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zyka.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Reservation()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        //public IActionResult Reservation()
        //{
        //    return View();
        //}

    }


}
