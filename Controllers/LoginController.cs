using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using System.Linq;

namespace CarRentalApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly CarRentalDbContext _context;

        public LoginController(CarRentalDbContext context)
        {
            _context = context;
        }

        // GET: Login
        [Route("/Login")]
        public ActionResult Index()
        {
            // Retrieve list of users from the database
            var users = _context.Users.ToList();

            // Pass the list of users to the view
            ViewBag.Users = users;

            return View();
        }
        //LoginPage
        public ActionResult LoggedIn()
        {
            return View();
        }

        public ActionResult LoggedOut()
        {
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public ActionResult Authenticate(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Perform authentication logic by querying the Users table
                var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Username && u.Password == loginModel.Password);

                if (user != null)
                {
                    // Authentication successful, redirect to loggedIn action
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    // Authentication failed, display error message
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                // Model validation failed, return to login page with error messages
                return RedirectToAction("Index");
            }
        }
    }
}
