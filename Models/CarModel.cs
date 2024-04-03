using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models // Change to your actual namespace
{
    public class CarModel
    {
        [Key]
        public int CarId { get; set; }

        [Required, StringLength(100)]
        public string Make { get; set; }

        [Required, StringLength(100)]
        public string Model { get; set; }

        [Required, StringLength(20)]
        public string Type { get; set; }

        [Range(4, 8)]
        public int Capacity { get; set; }
        
        [Range(1450, 2050)]
        public int Year
        {
            get; set;
        }

        [Required, StringLength(20)]
        public string LicensePlate { get; set; }

        [Required, Range(0, 1000)]
        public double DailyRate
        {
            get; set;
        }

        [Required, StringLength(50)]
        public string IsAvailable
        {
            get; set;
        }
    }
}
