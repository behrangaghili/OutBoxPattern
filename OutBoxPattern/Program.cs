//using DispacherApplication.Services;
using DispacherApplication.Broker;
using DispacherApplication.Persistence;
using DispacherApplication.Services;
using DispatcherService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using Serilog.Events;
using Serilog;
using DispacherApplication;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContextFactory<OrderOutBoxContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    });


// Add your services and repositories

// Add RabbitMQ and other configurations

builder.Services.AddTransient<IMessageBrokerClient, MessageBrokerClientService>();
builder.Services.AddTransient<RabbitMQClientManager>();
builder.Services.AddTransient<IDispatcherService, OrderDispatchService>();
builder.Services.AddHostedService<DispatcherBackgroundService>();

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // <-- Change this line!

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


// Global error handling middleware (optional)
// app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();
