using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Order.API.Repositories;
using Order.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IToDoItemValidator, ToDoItemValidator>();
builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

builder.Services.AddDbContext<ToDoItemDbContext>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {

        var meterName = builder.Configuration.GetValue<string>("MeterName")
                        ?? throw new InvalidOperationException("Unable to locate Otel meter name.");

        var otelEndpoint = builder.Configuration["Otel:Endpoint"]
                           ?? throw new InvalidOperationException("Otel endpoint was not configured.");

        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.GatewayAPI"))
            .AddMeter(meterName)
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(otelEndpoint);
            });
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WEB API",
        Version = "v1"
    });
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OPEN API"); // http://localhost:5000/swagger
        c.DocumentTitle = "WEB API";
        c.DocExpansion(DocExpansion.List);
    });
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();