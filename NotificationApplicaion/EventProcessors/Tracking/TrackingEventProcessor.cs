using NotificationMicroservice.EventProcessors.Order;
using System.Text.Json;

namespace NotificationMicroservice.EventProcessors.Tracking
{
    public class TrackingEventProcessor : IEventProcessor
    {
        public string EventType => "status_changed";

        public Task Process(string messageId, string subEventType, byte[] payload)
        {
            var orderEvent = JsonSerializer.Deserialize<TrackingOutboxMessage>(payload);
            return Task.CompletedTask;
        }
    }
}