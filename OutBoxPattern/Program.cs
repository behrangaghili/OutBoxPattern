//using DispacherApplication.Services;
using DispacherApplication.Persistence;
using DispatcherService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OrderOutBoxContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    });
 

// Add your services and repositories
 
// Add RabbitMQ and other configurations
builder.Services.Configure<RabbitMqConnectionOptions>(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.AddHostedService<DispatcherServices>();
builder.Services.AddSingleton< MessageBrokerClientService>();
builder.Services.AddSingleton<DispatcherBackgroundService>();
builder.Services.AddSingleton<RabbitMQClientManager>();

var app = builder.Build();

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
