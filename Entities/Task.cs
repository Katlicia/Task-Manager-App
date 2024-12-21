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
        public string? Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; } = false;

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
         
        // Foreign keys for UserAccount
        public int UserId { get; set; }
        public UserAccount User { get; set; }

        // Relation with UserAccount
        public ICollection<TaskUser> TaskUsers { get; set; }

    }
}
