using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;


namespace CarRentalApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly LMSDbContext _context;

        public ReservationController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: /Borrowing
        [HttpGet]
        [Route("/Reservation")]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _context.Reservations.ToListAsync();
            return View(reservations);
        }

        // GET: /Borrowing/{id}
        [HttpGet]
        [Route("/Reservation/{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                return View(reservation);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Reservation/Add")]
        public IActionResult AddReservation()
        {
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "Name");
            return View();
        }

        [HttpPost]
        [Route("/Reservation/Add")]
        public async Task<IActionResult> AddReservation(ReservationModel reservation)
        {
            ModelState.Remove("Car");
            ModelState.Remove("Renter");

            if (ModelState.IsValid)
            {
                // Retrieve the related CarModel and RenterModel based on the IDs.
                reservation.Car = await _context.Cars.FindAsync(reservation.CarId);
                reservation.Renter = await _context.Renters.FindAsync(reservation.RenterId);

                // Check if both the Book and Reader with the given IDs were found.
                if (reservation.Car == null || reservation.Renter == null)
                {
                    // Handle the case where the book or reader doesn't exist.
                    // Add error messages to ModelState or return a suitable response.
                    ModelState.AddModelError("", "The car or renter does not exist.");
                    // Re-populate ViewData as it was in the GET method for AddBorrowing.
                    ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "Model");
                    ViewData["RenterId"] = new SelectList(_context.Renters, "RenterId", "FName");
                    return View(reservation);
                }

                reservation.Car.IsAvailable = "No";
                // If both entities exist, then proceed to add the borrowing to the database.
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAllReservations");
            }
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"Error in {entry.Key}:");
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($" - {error.ErrorMessage}");
                        }
                    }
                }
            }

            // If model state is not valid, return the view with the current model to show validation errors.
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "Model", reservation.CarId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "RenterId", "FName", reservation.RenterId);
            return View(reservation);
        }

        [HttpGet]
        [Route("/Reservation/Update/{id}")]
        public async Task<IActionResult> UpdateReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                return View(reservation);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Reservation/Update/{id}")]
        public async Task<IActionResult> UpdateBorrowing(int id, ReservationModel updatedReservation)
        {
            ModelState.Remove("Car");
            ModelState.Remove("Renter");

            if (!ModelState.IsValid)
            {
                return View(updatedReservation);
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound($"No reservation found with ID {id}.");
            }

            // Assuming that ReservationModel includes BookId, ReaderId, BorrowDate, and ReturnDate
            // Update properties with values from updatedBorrowing
            reservation.CarId = updatedReservation.CarId;
            reservation.RenterId = updatedReservation.RenterId;
            reservation.BorrowDate = updatedReservation.BorrowDate;
            reservation.ReturnDate = updatedReservation.ReturnDate;
            reservation.Status = updatedReservation.Status;
            reservation.TaxRate = updatedReservation.TaxRate;
            reservation.TotalAmount = updatedReservation.TotalAmount;
            reservation.ReserveDate = updatedReservation.ReserveDate;
         
            // If the borrowing is being returned, you might want to update the book's availability
            if (updatedReservation.ReturnDate.HasValue)
            {
                var car = await _context.Cars.FindAsync(updatedReservation.CarId);
                if (car != null)
                {
                    // Assuming there's a property in CarModel to mark availability
                    car.IsAvailable = "Yes"; // Or however you mark availability in your model
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllReservations");
        }



        [HttpPost]
        [Route("/Reservation/Delete/{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAllReservations");
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Reservation/Overdue")]
        public async Task<IActionResult> GetOverdueReservations()
        {
            var cutoffDate = DateTime.Now.AddDays(-21); // Calculate the cutoff date

            var overdueReservations = await _context.Reservations
                .Where(b => b.ReturnDate == null && b.BorrowDate < cutoffDate)
                .ToListAsync();

            return View(overdueReservations);
        }


        [HttpGet]
        [Route("/Reservation/ByRenter")]
        public async Task<IActionResult> GetReservationsByRenter(int id)
        {
            var reservations = await _context.Reservations
                .Where(b => b.RenterId == id)
                .ToListAsync();

            if (reservations.Any())
            {
                return View(reservations);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Reservation/Return/{id}")]
        public async Task<IActionResult> ReturnReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound($"No borrowing found with ID {id}.");
            }

            reservation.ReturnDate = DateTime.Now;
            // Update the book's availability
            var car = await _context.Cars.FindAsync(reservation.CarId);
            if (car != null)
            {
                // Assuming there's a property in CarModel to mark availability
                car.IsAvailable = "Yes";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllReservation");
        }
    }
}

