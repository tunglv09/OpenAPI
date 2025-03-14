using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Swashbuckle.AspNetCore.SwaggerUI;
using Newtonsoft.Json.Serialization;
using Order.API.Configuration;
using Order.API.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var appName = builder.Configuration.GetValue<string>("APPLICATION_NAME");
var svcName = builder.Configuration.GetValue<string>("SERVICE_NAME");

builder.Services.AddDbContext<ToDoItemDbContext>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {
        var meterName = builder.Configuration.GetValue<string>("MeterName");
        var otelEndpoint = builder.Configuration["Otel:Endpoint"];

        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.GatewayAPI"))
            .AddMeter(meterName ?? "")
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(otelEndpoint ?? "");
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

builder.Services.AddCustomLogging(builder.Configuration, builder.Host, svcName, appName);

builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Base on applicationUrl from launchSettings. http://localhost:5000/swagger
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OPEN API");
        c.DocumentTitle = "WEB API";
        c.DocExpansion(DocExpansion.List);
    });
}

app.UseSerilogRequestLogging(opt =>
{
    opt.IncludeQueryInRequestPath = true;
}); 
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();