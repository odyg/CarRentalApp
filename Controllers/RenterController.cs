using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    public class RenterController : Controller
    {
        private readonly CarRentalDbContext _context;

        public RenterController(CarRentalDbContext context)
        {
            _context = context;
        }

        // GET: /Reader
        [HttpGet]
        [Route("/Renter")]
        public async Task<IActionResult> GetAllRenters()
        {
            var renters = await _context.Renters.ToListAsync(); // Directly query the database
            return View(renters); // Return the list as a View
        }

        // GET: /Reader/{id}
        [HttpGet]
        [Route("/Renter/Id")]
        public async Task<IActionResult> GetRenterById(int id) // Changed string id to int id for simplicity
        {
            var renter = await _context.Renters.FindAsync(id);
            if (renter != null)
            {
                return View(new List<RenterModel> { renter }); // Return the specific reader as a View
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Renter/Add")]
        public IActionResult AddRenter()
        {
            return View();
        }

        [HttpPost]
        [Route("/Renter/Add")]
        public async Task<IActionResult> AddReader(RenterModel renter)
        {
            if (ModelState.IsValid)
            {
                await _context.Renters.AddAsync(renter); // Add the reader to the database
                await _context.SaveChangesAsync(); // Save changes
                return RedirectToAction("GetAllRenters"); // Redirect to the list view
            }
            return View(renter);
        }

        [HttpGet]
        [Route("/Renter/Update/{id}")]
        public async Task<IActionResult> UpdateRenter(int id)
        {
            var renter = await _context.Renters.FindAsync(id);
            if (renter != null)
            {
                return View(renter);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("/Renter/Update/{id}")]
        public async Task<IActionResult> UpdateRenter(int id, RenterModel updatedRenter)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedRenter);
            }

            if (id != updatedRenter.RenterId)
            {
                return BadRequest("ID mismatch");
            }

            var renterInDb = await _context.Renters.FirstOrDefaultAsync(r => r.RenterId == id);
            if (renterInDb == null)
            {
                return NotFound($"No renter found with ID {id}");
            }

            // Manually update the properties
            renterInDb.FName = updatedRenter.FName;
            renterInDb.LName = updatedRenter.LName;
            renterInDb.Address = updatedRenter.Address;
            renterInDb.PhoneNumber = updatedRenter.PhoneNumber;
            renterInDb.Email = updatedRenter.Email;
            renterInDb.ZipCode = updatedRenter.ZipCode;
            // Apply any other property updates here

            await _context.SaveChangesAsync(); // This saves the changes to the database

            return RedirectToAction("GetAllRenters");
        }


        [HttpPost]
        [Route("/Renter/Delete/{id}")]
        public async Task<IActionResult> DeleteRenter(int id)
        {
            var renter = await _context.Renters.FindAsync(id);
            if (renter == null)
            {
                return NotFound();
            }

            // This is where you need to include the related BorrowedBooks to check if any exist. I HAVE TO DOUBLE CHECK THIS
            _context.Entry(renter).Collection(r => r.RentedCar).Load();

            if (renter.RentedCar != null && renter.RentedCar.Count > 0)
            {
                TempData["Warning"] = "Cannot delete renter with rented car.";
                return RedirectToAction("GetAllRenters");
            }

            _context.Renters.Remove(renter);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Renter deleted successfully.";
            return RedirectToAction("GetAllRenters");
        }


        // Adjust according to your actual data model and requirements
        [HttpGet]
        [Route("/Renter/ZipCode")]
        public async Task<IActionResult> GetRenterByZipCode(string zipcode)
        {
            int zip = int.Parse(zipcode);
            // Assuming ZipCode is a property in your RenterModel
            var renters = await _context.Renters.Where(r => r.ZipCode == zip).ToListAsync();
            if (renters.Any())
            {
                return View("GetRenterByZipcode", renters); // Reuse the GetAllReaders view
            }
            return NotFound();
        }

        [HttpGet]
        [Route("/Renter/{id}/RentedCar")]
        public async Task<IActionResult> GetRentedCar(int id)
        {
            var renter = await _context.Renters
                                       .Include(r => r.RentedCar)
                                       .FirstOrDefaultAsync(r => r.RenterId == id);

            if (renter != null)
            {
                // Assuming BorrowedBooks is a collection you want to display
                return View(renter.RentedCar);
            }

            return NotFound();
        }
    }
}

