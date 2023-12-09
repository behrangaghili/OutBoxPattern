using Microsoft.EntityFrameworkCore;
using OutBoxPattern.Contract;
using OutBoxPattern.Persistence.OutBoxPattern.Data;
using OutBoxPattern.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OutBoxContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    });



// Add Interaces
builder.Services.AddTransient<IEmailClient>();
builder.Services.AddTransient<IMessageConsumer>();
builder.Services.AddTransient<IMessageProducer>();
builder.Services.AddTransient<IOrderOutboxRepository>();
builder.Services.AddTransient<IOrderRepository>();
builder.Services.AddTransient<IOrderService>();


// Add your services and repositories
builder.Services.AddHostedService<MessageBrokerService>();
//builder.Services.AddTransient<NotificationConsumer>();
builder.Services.AddTransient<NotificationService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<OrderServicePublisher>();

// Add RabbitMQ and other configurations
 builder.Services.Configure<RabbitMqConnectionOptions>(builder.Configuration.GetSection("RabbitMQ"));

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
