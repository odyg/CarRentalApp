using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class RenterModel
    {
        [Key]
        public int RenterId
        {
            get; set;
        }
        [Required, StringLength(100)]
        public string FName
        {
            get; set;
        }
        [Required, StringLength(100)]
        public string LName
        {
            get; set;
        }

        [Required, StringLength(100)]
        public string Address
        {
            get; set;
        }

        [Required, StringLength(100)]
        public string PhoneNumber
        {
            get; set;
        }

        [Required, EmailAddress]
        public string Email
        {
            get; set;
        }

        [Required]
        public int ZipCode
        {
            get; set;
        }
        // Assuming you have a way to connect BorrowingModels to this reader
        //public List<ReserveModel> RentedCars { get; set; } = new List<ReserveModel>();
    }
}
