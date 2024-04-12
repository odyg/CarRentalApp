using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;

namespace CarRentalApp.Controllers
{
    public class LoginController : Controller
    {
        //LoginPage
        public ActionResult LoggedIn()
        {
            return View();
        }

        public ActionResult LoggedOut()
        {
            return View();
        }

        // GET: Login
        [Route("/Login")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public ActionResult Authenticate(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Here you can perform authentication logic
                // For simplicity, let's assume authentication is successful if username is "admin" and password is "password"
                if (loginModel.Username == "admin" && loginModel.Password == "password")
                {
                    // Authentication successful, redirect to dashboard or desired page
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    // Authentication failed, display error message
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View("Index", loginModel);
                }
            }
            else
            {
                // Model validation failed, return to login page with error messages
                return View("Login", loginModel);
            }
        }
    }
}
