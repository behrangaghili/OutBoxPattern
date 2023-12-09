using System;
using OrderApplicaion.Models;
using System.Threading.Tasks;
using OrderApplicaion.Persistence;
using OrderApplicaion.Contract;
using Microsoft.EntityFrameworkCore;

namespace OrderApplicaion.Services
{
    public class OrderService : IOrderService
    {
        private readonly OutBoxContext _context;
        private readonly IMessageProducer _publisher;

        public OrderService(OutBoxContext context, IMessageProducer publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task CreateOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var outboxEvent = new OutboxEventModel
            {
                EventId = Guid.NewGuid(),
                EventType = "OrderCreated",
                EventData = $"Order with ID {order.Id} created",
                CreatedOn = DateTime.UtcNow
            };

            _context.OutboxEvents.Add(outboxEvent);
            await _context.SaveChangesAsync();
            // Publish message
            var messageBody = CreateRabbitMqMessageBody(order); // Ensure this method creates the desired message format
            _publisher.Publish("order.created", messageBody);
        }

        public async Task EditOrder(OrderModel order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            var outboxEvent = new OutboxEventModel
            {
                EventId = Guid.NewGuid(),
                EventType = "OrderEdited",
                EventData = $"Order with ID {order.Id} edited",
                CreatedOn = DateTime.UtcNow
            };

            _context.OutboxEvents.Add(outboxEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                var outboxEvent = new OutboxEventModel
                {
                    EventId = Guid.NewGuid(),
                    EventType = "OrderDeleted",
                    EventData = $"Order with ID {orderId} deleted",
                    CreatedOn = DateTime.UtcNow
                };

                _context.OutboxEvents.Add(outboxEvent);
                await _context.SaveChangesAsync();
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
        public string CreateRabbitMqMessageBody(OrderModel order)
        {
            if (order == null) return null;

            var body = string.Format("orderId={0}, orderAmount={1}", order.Id, order.TotalAmount);
            return body;
        }

    }
}
