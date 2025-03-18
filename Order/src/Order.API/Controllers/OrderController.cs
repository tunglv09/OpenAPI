using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(ILogger<OrderController> logger) : ControllerBase
{
    [HttpGet]
    [Route("GetAll")]
    public Task<IActionResult> GetAll()
    {
        logger.LogInformation("Start getting all orders.");
        
        var order1 = new Models.Order
        {
            Id = 1,
            Name = "Order 1",
        };

        var order2 = new Models.Order
        {
            Id = 2,
            Name = "Order 2",
        };

        var list = new List<Models.Order> { order1, order2 };
        
        return Task.FromResult<IActionResult>(Ok(list));
    }
}