using Microsoft.EntityFrameworkCore;
using DispatcherService.Contract;
using DispatcherService.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ;
using System.Configuration;
using DispatcherService.Services;
using DispatcherService.Models;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RabbitMQConfigModel>(configuration.GetSection("RabbitMQ"));
builder.Services.AddTransient<OrderServiceMessagePublisher>();
//builder.Services.AddTransient<IOrderService, OrderService>();
 builder.Services.AddTransient<OrderService>();
builder.Services.AddDbContext<OutBoxContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
