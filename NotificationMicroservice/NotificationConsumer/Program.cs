using NotificationMicroservice;
using NotificationMicroservice.Application;
using NotificationMicroservice.EventProcessors;
using NotificationMicroservice.EventProcessors.Order;
using NotificationMicroservice.EventProcessors.Tracking;
using NotificationMicroservice.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEventMessageQueue, EventMessageQueue>();
builder.Services.AddTransient<ITextMessageProvider, KavengarMessage>();
builder.Services.AddTransient<IEventProcessor, OrderEventProcessor>();
builder.Services.AddTransient<IEventProcessor, TrackingEventProcessor>();
builder.Services.AddTransient<EventProcessorFactory>();
/*
builder.Services.AddTransient<IEnumerable<IEventProcessor>>(provider =>
{
    return provider.GetServices<IEventProcessor>();
});
*/
builder.Services.AddHostedService<EventBackgroundService>();

var app = builder.Build();

app.Run();