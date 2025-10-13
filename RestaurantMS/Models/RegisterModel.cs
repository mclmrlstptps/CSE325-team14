using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(password), ErrorMessage = "Passwords do not match.")]
        public string confirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        public string role { get; set; } = string.Empty;
    }
}
