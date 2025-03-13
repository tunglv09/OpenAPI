using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Repositories
{
    public class ToDoItemDbContext : DbContext
    {
        public ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
