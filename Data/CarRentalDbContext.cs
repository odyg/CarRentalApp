﻿using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


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

            modelBuilder.Entity<CarModel>().HasData(
               new CarModel { CarId = 1, Make = "Chevrolet", Model = "Bolt EV", Type = "Hatchback", Capacity = 4, Year = 2022, LicensePlate = "abc032", DailyRate = 55, IsAvailable = "TRUE" }, new CarModel { CarId = 2, Make = "Acura", Model = "MDX", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc035", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 3, Make = "Subaru", Model = "BRZ", Type = "Coupe", Capacity = 4, Year = 2022, LicensePlate = "abc036", DailyRate = 40, IsAvailable = "TRUE" }, new CarModel { CarId = 4, Make = "Jeep", Model = "Grand Wagoneer", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc065", DailyRate = 45, IsAvailable = "TRUE" }, new CarModel { CarId = 5, Make = "GMC", Model = "Hummer EV", Type = "Pickup", Capacity = 4, Year = 2022, LicensePlate = "abc075", DailyRate = 60, IsAvailable = "TRUE" }, new CarModel { CarId = 6, Make = "Chevrolet", Model = "Traverse", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc084", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 7, Make = "Chevrolet", Model = "Equinox", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc088", DailyRate = 55, IsAvailable = "TRUE" }, new CarModel { CarId = 8, Make = "Kia", Model = "Sorento", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc119", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 9, Make = "Nissan", Model = "Pathfinder", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc122", DailyRate = 50, IsAvailable = "TRUE" }, new CarModel { CarId = 10, Make = "Mercedes-Benz", Model = "C-Class", Type = "Coupe", Capacity = 4, Year = 2022, LicensePlate = "abc178", DailyRate = 50, IsAvailable = "TRUE" }, new CarModel { CarId = 11, Make = "Hyundai", Model = "Tucson", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc203", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 12, Make = "Volkswagen", Model = "Tiguan", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc268", DailyRate = 45, IsAvailable = "TRUE" }, new CarModel { CarId = 13, Make = "Mitsubishi", Model = "Eclipse Cross", Type = "SUV", Capacity = 6, Year = 2022, LicensePlate = "abc293", DailyRate = 65, IsAvailable = "TRUE" }, new CarModel { CarId = 14, Make = "Honda", Model = "Civic", Type = "Sedan", Capacity = 4, Year = 2022, LicensePlate = "abc299", DailyRate = 60, IsAvailable = "TRUE" }, new CarModel { CarId = 15, Make = "Nissan", Model = "Frontier Crew Cab", Type = "Pickup", Capacity = 4, Year = 2022, LicensePlate = "abc375", DailyRate = 55, IsAvailable = "TRUE" }, new CarModel { CarId = 16, Make = "Audi", Model = "A6 allroad", Type = "Wagon", Capacity = 4, Year = 2021, LicensePlate = "abc001", DailyRate = 40, IsAvailable = "TRUE" }, new CarModel { CarId = 17, Make = "Honda", Model = "Accord", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc003", DailyRate = 50, IsAvailable = "TRUE" }, new CarModel { CarId = 18, Make = "MAZDA", Model = "MAZDA3", Type = "Hatchback,Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc004", DailyRate = 55, IsAvailable = "TRUE" }, new CarModel { CarId = 19, Make = "Volkswagen", Model = "Atlas Cross Sport", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc005", DailyRate = 60, IsAvailable = "TRUE" }, new CarModel { CarId = 20, Make = "Porsche", Model = "Panamera", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc008", DailyRate = 40, IsAvailable = "TRUE" }, new CarModel { CarId = 21, Make = "Jeep", Model = "Grand Cherokee L", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc009", DailyRate = 45, IsAvailable = "TRUE" }, new CarModel { CarId = 22, Make = "Cadillac", Model = "XT6", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc014", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 23, Make = "Toyota", Model = "Corolla", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc015", DailyRate = 40, IsAvailable = "TRUE" }, new CarModel { CarId = 24, Make = "BMW", Model = "3 Series", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc019", DailyRate = 60, IsAvailable = "TRUE" }, new CarModel { CarId = 25, Make = "Kia", Model = "Stinger", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc021", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 26, Make = "Toyota", Model = "Tundra Double Cab", Type = "Pickup", Capacity = 4, Year = 2021, LicensePlate = "abc025", DailyRate = 55, IsAvailable = "TRUE" }, new CarModel { CarId = 27, Make = "Kia", Model = "Forte", Type = "Sedan", Capacity = 4, Year = 2021, LicensePlate = "abc026", DailyRate = 60, IsAvailable = "TRUE" }, new CarModel { CarId = 28, Make = "Subaru", Model = "Outback", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc028", DailyRate = 70, IsAvailable = "TRUE" }, new CarModel { CarId = 29, Make = "Cadillac", Model = "XT4", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc029", DailyRate = 40, IsAvailable = "TRUE" }, new CarModel { CarId = 30, Make = "Mercedes-Benz", Model = "G-Class", Type = "SUV", Capacity = 6, Year = 2021, LicensePlate = "abc031", DailyRate = 50, IsAvailable = "TRUE" }
           );

            // Seed data for RenterModel
            modelBuilder.Entity<RenterModel>().HasData(
                new RenterModel { RenterId = 1, FName = "Ken", LName = "Sánchez", Address = "1970 Napa Ct.", PhoneNumber = "697-555-0142", Email = "ken0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 2, FName = "Terri", LName = "Duffy", Address = "9833 Mt. Dias Blv.", PhoneNumber = "819-555-0175", Email = "terri0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 3, FName = "Roberto", LName = "Tamburello", Address = "7484 Roundtree Drive", PhoneNumber = "212-555-0187", Email = "roberto0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 4, FName = "Rob", LName = "Walters", Address = "9539 Glenside Dr", PhoneNumber = "612-555-0100", Email = "rob0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 5, FName = "Gail", LName = "Erickson", Address = "1226 Shoe St.", PhoneNumber = "849-555-0139", Email = "gail0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 6, FName = "Jossef", LName = "Goldberg", Address = "1399 Firestone Drive", PhoneNumber = "122-555-0189", Email = "jossef0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 7, FName = "Dylan", LName = "Miller", Address = "5672 Hale Dr.", PhoneNumber = "181-555-0156", Email = "dylan0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 8, FName = "Diane", LName = "Margheim", Address = "6387 Scenic Avenue", PhoneNumber = "815-555-0138", Email = "diane1@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 9, FName = "Gigi", LName = "Matthew", Address = "8713 Yosemite Ct.", PhoneNumber = "185-555-0186", Email = "gigi0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 10, FName = "Michael", LName = "Raheem", Address = "250 Race Court", PhoneNumber = "330-555-2568", Email = "michael6@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 11, FName = "Ovidiu", LName = "Cracium", Address = "1318 Lasalle Street", PhoneNumber = "719-555-0181", Email = "ovidiu0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 12, FName = "Thierry", LName = "D'Hers", Address = "5415 San Gabriel Dr.", PhoneNumber = "168-555-0183", Email = "thierry0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 13, FName = "Janice", LName = "Galvin", Address = "9265 La Paz", PhoneNumber = "473-555-0117", Email = "janice0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 14, FName = "Michael", LName = "Sullivan", Address = "8157 W. Book", PhoneNumber = "465-555-0156", Email = "michael8@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 15, FName = "Sharon", LName = "Salavaria", Address = "4912 La Vuelta", PhoneNumber = "970-555-0138", Email = "sharon0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 16, FName = "David", LName = "Bradley", Address = "40 Ellis St.", PhoneNumber = "913-555-0172", Email = "david0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 17, FName = "Kevin", LName = "Brown", Address = "6696 Anchor Drive", PhoneNumber = "150-555-0189", Email = "kevin0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 18, FName = "John", LName = "Wood", Address = "1873 Lion Circle", PhoneNumber = "486-555-0150", Email = "john5@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 19, FName = "Mary", LName = "Dempsey", Address = "3148 Rose Street", PhoneNumber = "124-555-0114", Email = "mary2@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 20, FName = "Wanida", LName = "Benshoof", Address = "6872 Thornwood Dr.", PhoneNumber = "708-555-0141", Email = "wanida0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 21, FName = "Terry", LName = "Eminhizer", Address = "5747 Shirley Drive", PhoneNumber = "138-555-0118", Email = "terry0@adventure-works.com", ZipCode = 98011 }, new RenterModel { RenterId = 22, FName = "Sariya", LName = "Harnpadoungsataya", Address = "636 Vine Hill Way", PhoneNumber = "399-555-0176", Email = "sariya0@adventure-works.com", ZipCode = 97205 }, new RenterModel { RenterId = 23, FName = "Mary", LName = "Gibson", Address = "6657 Sand Pointe Lane", PhoneNumber = "531-555-0183", Email = "mary0@adventure-works.com", ZipCode = 98104 }, new RenterModel { RenterId = 24, FName = "Jill", LName = "Williams", Address = "80 Sunview Terrace", PhoneNumber = "510-555-0121", Email = "jill0@adventure-works.com", ZipCode = 55802 }, new RenterModel { RenterId = 25, FName = "James", LName = "Hamilton", Address = "9178 Jumping St.", PhoneNumber = "870-555-0122", Email = "james1@adventure-works.com", ZipCode = 75201 }, new RenterModel { RenterId = 26, FName = "Peter", LName = "Krebs", Address = "5725 Glaze Drive", PhoneNumber = "913-555-0196", Email = "peter0@adventure-works.com", ZipCode = 94109 }, new RenterModel { RenterId = 27, FName = "Jo", LName = "Brown", Address = "2487 Riverside Drive", PhoneNumber = "632-555-0129", Email = "jo0@adventure-works.com", ZipCode = 84407 }, new RenterModel { RenterId = 28, FName = "Guy", LName = "Gilbert", Address = "9228 Via Del Sol", PhoneNumber = "320-555-0195", Email = "guy1@adventure-works.com", ZipCode = 85004 }, new RenterModel { RenterId = 29, FName = "Mark", LName = "McArthur", Address = "8291 Crossbow Way", PhoneNumber = "417-555-0154", Email = "mark1@adventure-works.com", ZipCode = 38103 }, new RenterModel { RenterId = 30, FName = "Britta", LName = "Simon", Address = "9707 Coldwater Drive", PhoneNumber = "955-555-0169", Email = "britta0@adventure-works.com", ZipCode = 32804 }
            );
        }

    }
}
