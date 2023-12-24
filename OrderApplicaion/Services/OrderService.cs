using OrderMicroservice.Models;
using OrderMicroservice.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace OrderMicroservice.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<OrderService> _logger; // Logger

        public OrderService(OrderDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new order and raise a create order event to outbox
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task CreateOrder(OrderModel order)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Orders.Add(order);

                var createOrderEvent = new OutboxEventModel
                {
                    EventId = Guid.NewGuid(),
                    EventType = "OrderCreated",
                    EventData = $"Order with ID {order.Id} created",
                    CreatedOn = DateTime.UtcNow,
                    Payload = JsonSerializer.Serialize(order),
                    PublishedOn = null
                };

                _context.OutboxEvents.Add(createOrderEvent);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Handle exceptions related to database operations
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Delete an order and raise delete outbox event
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            using var transaction = _context.Database.BeginTransaction();

            if (order != null)
            {
                var deletOrderEvent = new OutboxEventModel
                {
                    EventId = Guid.NewGuid(),
                    EventType = "OrderDeleted",
                    EventData = $"Order with ID {orderId} deleted",
                    CreatedOn = DateTime.UtcNow,
                    Payload = JsonSerializer.Serialize(order),
                    PublishedOn = null
                };

                _context.Orders.Remove(order);
                _context.OutboxEvents.Add(deletOrderEvent);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task<OrderModel> GetOrderById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            return order; // Simply return the found order, or null if not found
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
