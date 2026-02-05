using System.ComponentModel.DataAnnotations;

namespace booker.api.Models.DTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "must be required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}