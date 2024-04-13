using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class UserModel
    {
        [Key]
        public string Username { get; set; }
        [Required, StringLength(100)]
        public string Password { get; set; }
        [Required, StringLength(100)]
        public string Email { get; set; }
    }
}
