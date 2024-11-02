using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Max 20 or min 6 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
