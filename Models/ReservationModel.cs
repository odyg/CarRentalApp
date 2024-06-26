﻿using System.ComponentModel.DataAnnotations;


namespace CarRentalApp.Models
{
    public class ReservationModel
    {
        [Key]
        public int ReservationId
        {
            get; set;
        }

        [Required]
        public int CarId
        {
            get; set;
        } // Foreign key from Book model

        [Required]
        public int RenterId
        {
            get; set;
        } // Foreign key from Reader model

        [DataType(DataType.Date)]
        public DateTime BorrowDate
        {
            get; set;
        }

        [DataType(DataType.Date)]
        public DateTime ReserveDate
        {
            get; set;
        }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate
        {
            get; set;
        } // Nullable for cases when the book hasn't been returned yet

        [Required]
        public string Status
        {
            get; set;
        }

        public double TaxRate
        {
            get; set;
        }

        public double TotalAmount
        {
            get; set;
        }

        // Navigation properties
        public CarModel Car
        {
            get; set;
        }
        public RenterModel Renter
        {
            get; set;
        }
    }
}