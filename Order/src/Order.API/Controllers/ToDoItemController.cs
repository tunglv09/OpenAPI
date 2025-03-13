using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Services;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService toDoItemService;

        public ToDoItemController(IToDoItemService toDoItemService)
        {
            this.toDoItemService = toDoItemService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await toDoItemService.GetAll());
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            return Ok(await toDoItemService.Get(Id));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Post(ToDoItem item)
        {
            await toDoItemService.Insert(item);
            return Ok();
        }


        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Put(ToDoItem item)
        {
            await toDoItemService.Update(item);
            return Ok();
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            var result = toDoItemService.Delete(id);
            return new JsonResult(result);
        }
    }
}
