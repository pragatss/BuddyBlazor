using System.ComponentModel.DataAnnotations;

namespace BuddyBlazor.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(15, ErrorMessage = "Must be between 5 and 15 characters", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        public DateTime? BirthDate { get; set; }
    }
}
