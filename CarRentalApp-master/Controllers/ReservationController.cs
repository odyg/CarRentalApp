using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using CsvHelper;
using System.Globalization;
using System.Text;


namespace CarRentalApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CarRentalDbContext _context;

        public ReservationController(CarRentalDbContext context)
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
            
            return View();
        }

        [HttpGet]
        [Route("/Reservation/AddFromCarView/{id}")]
        public async Task<IActionResult> AddReservationFromCarView(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound($"No car found with ID {id}.");
            }

            var reservation = new ReservationModel
            {
                CarId = car.CarId,
            };

            return View(reservation);
        }

        [HttpPost]
        [Route("/Reservation/AddFromCarView/{id}")]
        public async Task<IActionResult> AddReservationFromCarView(int id, ReservationModel reservation)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound($"No car found with ID {id}.");
            }

            //var reservation = new ReservationModel
            //{
            //    CarId = car.CarId,
            //};

            var renter = await _context.Renters.FindAsync(reservation.RenterId);
            reservation.Renter = renter;

            var days = (reservation.ReturnDate - reservation.BorrowDate).Value.Days;
            var dailyRate = car.DailyRate;
            var taxRate = 0.1; // Hardcoded tax rate for now
            reservation.TotalAmount = (dailyRate * days) + (dailyRate * days * taxRate);

            return View("ConfirmReservation", reservation);
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

                // Calculate the TotalAmount based on the days between BorrowDate and ReturnDate,
                // DailyRate from the CarModel, and a hardcoded TaxRate.
                var days = (reservation.ReturnDate - reservation.BorrowDate).Value.Days;
                var dailyRate = reservation.Car.DailyRate;
                var taxRate = 0.1; // Hardcoded tax rate for now
                reservation.TotalAmount = (dailyRate * days) + (dailyRate * days * taxRate);

                // Pass the reservation model to the view for confirmation.
                return View("ConfirmReservation", reservation);

                //reservation.Car.IsAvailable = "No";
                //// If both entities exist, then proceed to add the borrowing to the database.
                //await _context.Reservations.AddAsync(reservation);
                //await _context.SaveChangesAsync();
                //return RedirectToAction("GetAllReservations");
            }

            // If model state is not valid, return the view with the current model to show validation errors.
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "Model", reservation.CarId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "RenterId", "FName", reservation.RenterId);
            return View(reservation);
        }

        [HttpPost]
        [Route("/Reservation/Confirm")]
        public async Task<IActionResult> ConfirmReservation(ReservationModel reservation)
        {
            ModelState.Remove("Car");
            ModelState.Remove("Renter");

            if (ModelState.IsValid)
            {
                // Add the reservation to the database
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();

                // Update the car's availability
                var car = await _context.Cars.FindAsync(reservation.CarId);
                
                    // Assuming there's a property in CarModel to mark availability
                    car.IsAvailable = "No"; // Or however you mark availability in your model
                    await _context.SaveChangesAsync();
                

                // Redirect to a success page or display a success message
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
            return View("ConfirmReservation", reservation);
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
                return NotFound($"No reservation found with ID {id}.");
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
            return RedirectToAction("GetAllReservations");
        }

        
        [HttpGet]
        [Route("/Reports")]
        public IActionResult Reports(DateTime? startDate, DateTime? endDate, int? carId, int? renterId)
        {
            var reservations = _context.Reservations.AsQueryable();

            // Apply filters if they are provided
            if (startDate.HasValue && endDate.HasValue)
            {
                reservations = reservations.Where(r => r.BorrowDate >= startDate.Value && r.BorrowDate <= endDate.Value);
            }
            if (carId.HasValue)
            {
                reservations = reservations.Where(r => r.CarId == carId.Value);
            }
            if (renterId.HasValue)
            {
                reservations = reservations.Where(r => r.RenterId == renterId.Value);
            }

            return View(reservations.ToList());
        }

        public IActionResult ExportAsCsv()
        {
            var reservations = _context.Reservations.ToList();
            var csvBuilder = new StringBuilder();
            var stringWriter = new StringWriter(csvBuilder);

            using (var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(reservations);
            }

            return File(new UTF8Encoding().GetBytes(csvBuilder.ToString()), "text/csv", "Reservations.csv");
        }


    }
}

