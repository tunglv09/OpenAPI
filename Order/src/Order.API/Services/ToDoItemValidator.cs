using Order.API.Models;

namespace Order.API.Services
{
    public interface IToDoItemValidator
    {
        void ValidateToDoItem(ToDoItem item);
    }

    public class ToDoItemValidator : IToDoItemValidator
    {
        public void ValidateToDoItem(ToDoItem item)
        {
            if (item.DueDate is null)
            {
                throw new ArgumentException("Due date is required.");
            }

            if (item.DueDate < DateTimeOffset.UtcNow)
            {
                throw new InvalidOperationException("Due date must not be in the past");
            }
        }
    }
}
