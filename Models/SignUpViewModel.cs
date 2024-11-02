using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max 50 or min 3 characters allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max 50 or min 3 characters allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Max 20 or min 6 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
