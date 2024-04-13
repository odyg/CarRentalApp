using Microsoft.AspNetCore.Mvc;
using System;
using CarRentalApp.Models;
using CarRentalApp.Data;

namespace YourApplication.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly CarRentalDbContext _context;

        public RegistrationController(CarRentalDbContext context)
        {
            _context = context;
        }
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
                // Create a new user entity based on the registration model
                var newUser = new UserModel
                {
                    Username = registrationModel.Username,
                    Password = registrationModel.Password, // Note: You should hash the password before saving it
                    Email = registrationModel.Email
                };

                // Save the new user to the database
                try
                {
                    // Call your data access layer to save the user to the database
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    // After successful registration, redirect to login page or dashboard
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    ViewBag.ErrorMessage = "An error occurred while registering the user.";
                    return View("Index", registrationModel);
                }
            }
            else
            {
                // Model validation failed, return to registration page with error messages
                return View("Registration", registrationModel);
            }
        }
    }
}
