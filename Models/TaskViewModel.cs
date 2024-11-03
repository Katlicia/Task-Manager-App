using System.ComponentModel.DataAnnotations;
using TodoApp.Entities;

namespace TodoApp.Models
{
    public class TaskViewModel
    {
        [Required(ErrorMessage = "Task title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max 50 or min 3 characters allowed.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "Max 500 characters allowed.")]
        public string? Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
    }
}
