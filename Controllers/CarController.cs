using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;


namespace CarRentalApp.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly LMSDbContext _context;

        public CarController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: /Book
        [HttpGet]
        [Route("/Car")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _context.Car.ToListAsync(); 
            return View(cars);
        }

        [HttpGet]
        [Route("/Car/Id")]
        public async Task<IActionResult> GetCarById(int id) 
        {
            //int bid = int.Parse(id);
            var car = await _context.Cars.FirstOrDefaultAsync(b => b.CarId == id);
            if (car != null)
            {
                return View("GetCarById", car); // Display the single book view
            }
            return NotFound(); // Or return a NotFound result
        }

        [HttpGet]
        [Route("/Car/Add")]
        public IActionResult AddCar()
        {
            return View();
        }

        // POST: /Book/Add
        [HttpPost]
        [Route("/Car/Add")]
        public async Task<IActionResult> AddCar(CarModel car)
        {
            if (ModelState.IsValid)
            {
                await _context.Cars.AddAsync(car); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction("GetAllCars");
            }
            return View(car);
        }

        [HttpGet]
        [Route("/Car/Update/{id}")]
        public async Task<IActionResult> UpdateCar(int id)
        {

            var car = await _context.Cars.FindAsync(id);
            //var book = _context.Books.Find(id); // Find the book by ID
            if (car != null)
            {
                return View(car);
            }
            return NotFound();
        }

        // PUT: /Book/Update/{id}
        [HttpPost]
        [Route("/Car/Update/{id}")]
        public async Task<IActionResult> UpdateCar(int id, CarModel updatedCar)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCar);
            }

            var car = await _context.Cars.FirstOrDefaultAsync(b => b.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            // Update properties
            car.Make = updatedCar.Make;
            car.Model = updatedCar.Model;
            car.Type = updatedCar.Type;
            car.Capacity = updatedCar.Capacity;
            car.Year = updatedCar.Year;
            car.DailyRate = updatedCar.DailyRate;
            car.LicensePlate = updatedCar.LicensePlate;
            car.IsAvailable = updatedCar.IsAvailable;

            // The context automatically tracks these changes
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllCars");
        }


        // POST: /Book/Delete/{id}
        [HttpPost]
        [Route("/Car/Delete/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id); // Use FindAsync for asynchronous operation
            if (car == null)
            {
                return NotFound();
            }
            if(car.IsAvailable == "No")
            {

                TempData["Error"] = "Car is not available for deletion (it's rented).";
                return RedirectToAction("GetAllCars");
            }
            // ... Check if the book is available to be deleted ...

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync(); // Use SaveChangesAsync for asynchronous operation
            TempData["Success"] = "Car deleted successfully.";
            return RedirectToAction("GetAllCars");
        }

        [HttpGet]
        [Route("/Car/Genre")]
        public async Task<IActionResult> GetCarsByType(string type)
        {
            var cars = await _context.Cars.Where(b => b.Type == type).ToListAsync(); // Use ToListAsync for asynchronous operation
            if (cars.Any())
            {
                return View("GetAllCars", cars); // Return books of a specific genre as a View
            }
            return NotFound(); // Or return a NotFound result
        }
    }
}
