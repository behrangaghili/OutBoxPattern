
using DispacherApplication.Models;
using DispacherApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DispacherApplication.Services
{
    public class OrderDispatchService : IDispatcherService
    {
        private readonly IDbContextFactory<OrderOutBoxContext> _dbContextFactory;
        private readonly IMessageBrokerClient _messageBrokerClient;
        private readonly OrderOutBoxContext _dbContext;

        public OrderDispatchService(IDbContextFactory<OrderOutBoxContext> dbContextFactory, IMessageBrokerClient messageBrokerClient)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
            _messageBrokerClient = messageBrokerClient;
        }

        public async Task DispatchPendingEventsAsync(CancellationToken cancellationToken)
        {
            CheckConnection();
            PublishEvents();
        }

        private void PublishEvents()
        {
            var events = GetPendingEventOrders();

            foreach (var @event in events)
            {
                try
                {
                    _messageBrokerClient.Publish(@event.EventId.ToString(), @event.EventType, @event.Payload);
                    @event.PublishedOn = DateTime.UtcNow;
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Cannot save data to database!");
                }
            }
        }

        private List<OutboxEventModel> GetPendingEventOrders()
        {
            return _dbContext
                            .OutboxEvents
                            .Where(x => x.PublishedOn == null)
                            .ToList();
        }

        private void CheckConnection()
        {
            var isConnected = _messageBrokerClient.IsConnected();
            if (!isConnected) _messageBrokerClient.Connect();
        }

    }
}
