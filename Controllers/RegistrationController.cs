using Microsoft.AspNetCore.Mvc;
using System;
using CarRentalApp.Models;

namespace YourApplication.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [Route("/Registration")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/Registration")]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                // Save registration information to the database
                // You can write your code here to save registrationModel to the database

                // After successful registration, redirect to login page or dashboard
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // Model validation failed, return to registration page with error messages
                return View("Registration", registrationModel);
            }
        }
    }
}
