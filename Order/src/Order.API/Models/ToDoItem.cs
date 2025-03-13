using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    [Table("ToDoItem")]

    public class ToDoItem
    {
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTimeOffset? DueDate { get; set; }

        public int NumberOfDaysForToDoItemExpiredAfterDueDate { get; set; } = 10;

        public bool IsExpired { get; set; }
    }
}
