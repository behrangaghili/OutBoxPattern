using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OutBoxPattern.Contract;
using OutBoxPattern.Models;
// Add other necessary using statements here

namespace OutBoxPattern.Services
{
    public class MessageBrokerService : BackgroundService
    {
        private readonly IOrderOutboxRepository orderOutboxRepository;
        private readonly IMessageProducer rabbitMqProducer;
        private readonly ILogger<MessageBrokerService> logger;
        private readonly TimeSpan delayBetweenChecks;

        public MessageBrokerService(
            IOrderOutboxRepository orderOutboxRepository,
            IMessageProducer rabbitMqProducer,
            ILogger<MessageBrokerService> logger,
            IConfiguration configuration)
        {
            this.orderOutboxRepository = orderOutboxRepository;
            this.rabbitMqProducer = rabbitMqProducer;
            this.logger = logger;
            delayBetweenChecks = configuration.GetValue<TimeSpan>("OutboxProcessor:DelayBetweenChecks");
        }

        protected async Task Execute(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var outboxEvents = await orderOutboxRepository.GetUnpublishedEventsAsync();

                    foreach (var outboxEvent in outboxEvents)
                    {
                        try
                        {
                            var eventData = JsonConvert.DeserializeObject<OutboxEvent>(outboxEvent.EventData);
                            rabbitMqProducer.Publish(eventData.EventType, eventData);

                            outboxEvent.PublishedOn = DateTime.UtcNow;
                            await orderOutboxRepository.UpdateAsync(outboxEvent);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error publishing outbox event");
                            // Handle specific error scenarios
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing outbox events");
                }

                await Task.Delay(delayBetweenChecks, stoppingToken);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
