using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class RegisterUserDto
    {
        [Required]
        public int Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public int LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
