using Microsoft.AspNetCore.Mvc;

namespace Zyka_Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
