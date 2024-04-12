using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace CarRentalApp.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
        public DbSet<CarModel> Cars
        {
            get; set;
        }
        public DbSet<RenterModel> Renters
        {
            get; set;
        }
        public DbSet<ReservationModel> Reservations
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReservationModel>()
                .HasOne<CarModel>(b => b.Car) // Assuming there's a navigation property 'Book' in BorrowingModel
                .WithMany() // Assuming BookModel does not have a navigation property back to BorrowingModel
                .HasForeignKey(b => b.CarId);

            modelBuilder.Entity<ReservationModel>()
                .HasOne<RenterModel>(b => b.Renter) // Assuming there's a navigation property 'Reader' in BorrowingModel
                .WithMany(r => r.RentedCar) // Assuming ReaderModel has a navigation property 'BorrowedBooks'
                .HasForeignKey(b => b.RenterId);
        }

    }
}
