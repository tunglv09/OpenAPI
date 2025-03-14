using Order.API.Repositories;
using Order.API.Services;

namespace Order.API.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        services.AddScoped<IToDoItemValidator, ToDoItemValidator>();
        services.AddScoped<IToDoItemService, ToDoItemService>();
    }
}