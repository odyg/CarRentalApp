using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class LoginModel
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
