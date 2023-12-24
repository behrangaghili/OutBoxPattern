using Microsoft.EntityFrameworkCore;
using NotificationMicroservice.Application;
using NotificationMicroservice.Data;
using NotificationMicroservice.EventProcessors;
using NotificationMicroservice.EventProcessors.Order;
using NotificationMicroservice.EventProcessors.Tracking;
using NotificationMicroservice.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventMessageQueue, EventMessageQueue>();
builder.Services.AddSingleton<ITextMessageProvider, KavengarMessage>();
builder.Services.AddSingleton<EventProcessorFactory>();

builder.Services.AddTransient<IEventProcessor,OrderEventProcessor>();
builder.Services.AddTransient<IEventProcessor,TrackingEventProcessor>();

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<AppDataContext>(config =>
{
    config.UseSqlServer(connectionString);
});

var app = builder.Build();

Task.Run(() =>
{
    var notifManager = app.Services.GetService<IEventMessageQueue>();
    notifManager.Start();
});

app.MapGet("/weatherforecast", () =>
{
    return "ok";
});

app.Run();