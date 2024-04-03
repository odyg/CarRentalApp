using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


namespace CarRentalApp.Controllers
{
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
            var borrowings = await _context.Borrowings.ToListAsync();
            return View(borrowings);
        }

        // GET: /Borrowing/{id}
        [HttpGet]
        [Route("/Borrowing/{id}")]
        public async Task<IActionResult> GetBorrowingById(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing != null)
            {
                return View(borrowing);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Borrowing/Add")]
        public IActionResult AddBorrowing()
        {
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "Name");
            return View();
        }

        [HttpPost]
        [Route("/Borrowing/Add")]
        public async Task<IActionResult> AddBorrowing(ReservationModel borrowing)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Reader");

            if (ModelState.IsValid)
            {
                // Retrieve the related CarModel and ReaderModel based on the IDs.
                borrowing.Book = await _context.Books.FindAsync(borrowing.BookId);
                borrowing.Reader = await _context.Readers.FindAsync(borrowing.ReaderId);

                // Check if both the Book and Reader with the given IDs were found.
                if (borrowing.Book == null || borrowing.Reader == null)
                {
                    // Handle the case where the book or reader doesn't exist.
                    // Add error messages to ModelState or return a suitable response.
                    ModelState.AddModelError("", "The book or reader does not exist.");
                    // Re-populate ViewData as it was in the GET method for AddBorrowing.
                    ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
                    ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "Name");
                    return View(borrowing);
                }

                borrowing.Book.IsAvailable = "No";
                // If both entities exist, then proceed to add the borrowing to the database.
                await _context.Borrowings.AddAsync(borrowing);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAllBorrowings");
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
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", borrowing.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "Name", borrowing.ReaderId);
            return View(borrowing);
        }

        [HttpGet]
        [Route("/Borrowing/Update/{id}")]
        public async Task<IActionResult> UpdateBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing != null)
            {
                return View(borrowing);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Borrowing/Update/{id}")]
        public async Task<IActionResult> UpdateBorrowing(int id, ReservationModel updatedBorrowing)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Reader");

            if (!ModelState.IsValid)
            {
                return View(updatedBorrowing);
            }

            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing == null)
            {
                return NotFound($"No borrowing found with ID {id}.");
            }

            // Assuming that ReservationModel includes BookId, ReaderId, BorrowDate, and ReturnDate
            // Update properties with values from updatedBorrowing
            borrowing.BookId = updatedBorrowing.BookId;
            borrowing.ReaderId = updatedBorrowing.ReaderId;
            borrowing.BorrowDate = updatedBorrowing.BorrowDate;
            borrowing.ReturnDate = updatedBorrowing.ReturnDate;

            // If the borrowing is being returned, you might want to update the book's availability
            if (updatedBorrowing.ReturnDate.HasValue)
            {
                var book = await _context.Books.FindAsync(updatedBorrowing.BookId);
                if (book != null)
                {
                    // Assuming there's a property in CarModel to mark availability
                    book.IsAvailable = "Yes"; // Or however you mark availability in your model
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllBorrowings");
        }



        [HttpPost]
        [Route("/Borrowing/Delete/{id}")]
        public async Task<IActionResult> DeleteBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing != null)
            {
                _context.Borrowings.Remove(borrowing);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAllBorrowings");
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Borrowing/Overdue")]
        public async Task<IActionResult> GetOverdueBorrowings()
        {
            var cutoffDate = DateTime.Now.AddDays(-21); // Calculate the cutoff date

            var overdueBorrowings = await _context.Borrowings
                .Where(b => b.ReturnDate == null && b.BorrowDate < cutoffDate)
                .ToListAsync();

            return View(overdueBorrowings);
        }


        [HttpGet]
        [Route("/Borrowing/ByReader")]
        public async Task<IActionResult> GetBorrowingsByReader(int id)
        {
            var borrowings = await _context.Borrowings
                .Where(b => b.ReaderId == id)
                .ToListAsync();

            if (borrowings.Any())
            {
                return View(borrowings);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Borrowing/Return/{id}")]
        public async Task<IActionResult> ReturnBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing == null)
            {
                return NotFound($"No borrowing found with ID {id}.");
            }

            borrowing.ReturnDate = DateTime.Now;
            // Update the book's availability
            var book = await _context.Books.FindAsync(borrowing.BookId);
            if (book != null)
            {
                // Assuming there's a property in CarModel to mark availability
                book.IsAvailable = "Yes";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllBorrowings");
        }
    }
}

