
using DispacherApplication.Models;
using DispacherApplication.Persistence;

namespace DispacherApplication.Services
{
    public class DispatchService : IDispatcherService
    {
        private readonly OrderOutBoxContext _dbContext;
        private readonly IMessageBrokerClient _messageBrokerClient;

        public DispatchService(OrderOutBoxContext dbContext, IMessageBrokerClient messageBrokerClient)
        {
            _dbContext = dbContext;
            _messageBrokerClient = messageBrokerClient;
        }

        public async Task DispatchPendingEventsAsync(CancellationToken cancellationToken)
        {
            await CheckConnection();
            PublishEvents();
        }

        private void PublishEvents()
        {
            var pendingOrderEvents = GetPendingEventOrders();

            foreach (var order in pendingOrderEvents)
            {
                _messageBrokerClient.PublishAsync(order.EventId.ToString(), order.EventType, order.Payload);
                order.PublishedOn = DateTime.UtcNow;
                _dbContext.SaveChanges();
            }
        }

        private List<OutboxEventModel> GetPendingEventOrders()
        {
            return _dbContext
                            .OutboxEvents
                            .Where(x => x.PublishedOn == null)
                            .ToList();
        }

        private async Task CheckConnection()
        {
            var isConnected = await _messageBrokerClient.IsConnectedAsync();
            if (!isConnected) await _messageBrokerClient.ConnectAsync();
        }
    }
}
