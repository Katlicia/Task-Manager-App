using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string Password { get; set; }

        // Others
        public List<Task> Tasks { get; set; }
    }
}
