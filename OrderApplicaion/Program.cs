using Microsoft.EntityFrameworkCore;
using OrderApplicaion.Contract;
using OrderApplicaion.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ;
using System.Configuration;
using OrderApplicaion.Services;
using OrderApplicaion.Models;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RabbitMQConfig>(configuration.GetSection("RabbitMQ"));
builder.Services.AddTransient<IMessageProducer, OrderServicePublisher>();
builder.Services.AddTransient<IOrderService, OrderService>();
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
