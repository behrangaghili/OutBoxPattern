
using NotificationMicroservice.Application;

namespace NotificationMicroservice
{
    public class EventBackgroundService : BackgroundService
    {
        private readonly IEventMessageQueue eventMessageQueue;

        public EventBackgroundService(IEventMessageQueue eventMessageQueue)
        {
            this.eventMessageQueue = eventMessageQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                eventMessageQueue.Start();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
