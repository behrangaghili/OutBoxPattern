using NotificationMicroservice.Messaging;
using System.Text.Json;

namespace NotificationMicroservice.EventProcessors.Order
{
    public class OrderEventProcessor : IEventProcessor
    {
        private readonly ITextMessageProvider textMessageProvider;

        public OrderEventProcessor(ITextMessageProvider textMessageProvider)
        {
            this.textMessageProvider = textMessageProvider;
        }

        public string EventType => "create_order";

        public Task Process(string messageId, string subEventType, byte[] payload)
        {
            var orderEvent = JsonSerializer.Deserialize<OrderOutboxMessage>(payload);

            if (IsTehran(orderEvent))
                textMessageProvider.SendMessage("tehran message", orderEvent.CustomerMobileNo);
            else
                textMessageProvider.SendMessage("other city message", orderEvent.CustomerMobileNo);

            return Task.CompletedTask;
        }

        private static bool IsTehran(OrderOutboxMessage? orderEvent)
        {
            const int tehranCityId = 10;
            return orderEvent.FromCityID == tehranCityId;
        }
    }
}
