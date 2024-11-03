using System.ComponentModel.DataAnnotations;

namespace TodoApp.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Task title is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "Max 500 characters allowed.")]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        // Foreign keys for UserAccount
        public int UserId { get; set; }
        public UserAccount User { get; set; }

    }
}
