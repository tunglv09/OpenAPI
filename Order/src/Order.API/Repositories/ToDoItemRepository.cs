using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Repositories
{
    public interface IToDoItemRepository
    {
        Task<IEnumerable<ToDoItem>> GetAll();
        Task<ToDoItem> Get(string id);
        Task<ToDoItem> Insert(ToDoItem toDoItem);
        Task<ToDoItem> Update(ToDoItem toDoItem);
        void Delete(string id);
    }

    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ToDoItemDbContext toDoItemDbContext;

        public ToDoItemRepository(ToDoItemDbContext toDoItemDbContext)
        {
            this.toDoItemDbContext = toDoItemDbContext;
        }

        public async Task<IEnumerable<ToDoItem>> GetAll()
        {
            return await toDoItemDbContext.ToDoItems.ToListAsync();
        }

        public async Task<ToDoItem> Get(string id)
        {
            return await toDoItemDbContext.ToDoItems.FindAsync(id);
        }

        public async Task<ToDoItem> Insert(ToDoItem toDoItem)
        {
            toDoItem.Id = Guid.NewGuid().ToString();
            toDoItemDbContext.ToDoItems.Add(toDoItem);
            await toDoItemDbContext.SaveChangesAsync();
            return toDoItem;
        }

        public async Task<ToDoItem> Update(ToDoItem toDoItem)
        {
            toDoItemDbContext.Entry(toDoItem).State = EntityState.Modified;
            await toDoItemDbContext.SaveChangesAsync();
            return toDoItem;
        }

        public void Delete(string id)
        {
            var department = toDoItemDbContext.ToDoItems.Find(id);

            if (department != null)
            {
                toDoItemDbContext.Entry(department).State = EntityState.Deleted;
                toDoItemDbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"To do item id={id} does not exist.");
            }
        }
    }
}