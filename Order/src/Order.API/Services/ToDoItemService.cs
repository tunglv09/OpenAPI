using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Order.API.Models;
using Order.API.Repositories;

namespace Order.API.Services
{
    public interface IToDoItemService
    {
        string Delete(string id);
        Task<ToDoItem> Get(string id);
        Task<IEnumerable<ToDoItem>> GetAll();
        Task<ToDoItem> Insert(ToDoItem toDoItem);
        Task<ToDoItem> Update(ToDoItem toDoItem);
    }

    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository toDoItemRepository;
        private readonly IToDoItemValidator toDoItemValidator;
        private readonly ILogger<ToDoItemService> logger;

        public ToDoItemService(
            IToDoItemRepository toDoItemRepository,
            IToDoItemValidator toDoItemValidator,
            ILogger<ToDoItemService> logger)
        {
            this.toDoItemRepository = toDoItemRepository;
            this.toDoItemValidator = toDoItemValidator;
            this.logger = logger;
        }

        public async Task<IEnumerable<ToDoItem>> GetAll()
        {
            return await toDoItemRepository.GetAll();
        }

        public async Task<ToDoItem> Get(string id)
        {
            return await toDoItemRepository.Get(id);
        }

        public async Task<ToDoItem> Insert(ToDoItem toDoItem)
        {
            try
            {
                toDoItemValidator.ValidateToDoItem(toDoItem);
                await toDoItemRepository.Insert(toDoItem);

                return toDoItem;
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Failed to create the to do item {item} because validation failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Failed to create the to do item {item} because validation failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to create the to do item {item} because database persistence failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create the to do item {item}.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
        }

        public async Task<ToDoItem> Update(ToDoItem toDoItem)
        {
            try
            {
                toDoItemValidator.ValidateToDoItem(toDoItem);
                await toDoItemRepository.Update(toDoItem);

                return toDoItem;
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Failed to update the to do item {item} because argument validation failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Failed to update the to do item {item} because argument validation failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to update the to do item {item} because database persistence failed.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update the to do item {item}.", JsonConvert.SerializeObject(toDoItem));
                throw;
            }
        }

        public string Delete(string id)
        {
            try
            {
                toDoItemRepository.Delete(id);

                return $"Deleted to do item id={id}";
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Failed to delete to do item id={id} because the item does not exist.", id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to update the to do item id={id} because database persistence failed.", id);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete the to do item {id}.", id);
                throw;
            }
        }
    }
}
