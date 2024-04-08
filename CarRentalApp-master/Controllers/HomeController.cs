using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Home()
        {
            return View();
        }
    }
}
