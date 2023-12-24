using System;
using DispatcherService.Models;
using System.Threading.Tasks;
using DispatcherService.Persistence;
using DispatcherService.Contract;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DispatcherService.Services
{
    public class OrderService: IOrderService
    {
        private readonly OutBoxContext _context;
        private readonly OrderServiceMessagePublisher _publisher;
        private readonly ILogger<OrderService> _logger; // Logger
        public OrderService(OutBoxContext context, OrderServiceMessagePublisher publisher, ILogger<OrderService> logger)
        {
            _context = context;
            _publisher = publisher;
            _logger = logger;
        }
        public async Task CreateOrder(OrderModel order)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    var messageBody = CreateRabbitMqMessageBody(order);
                    var outboxEvent = new OutboxEventModel
                    {
                        EventId = Guid.NewGuid(),
                        EventType = "OrderCreated",
                        EventData = $"Order with ID {order.Id} created",
                        CreatedOn = DateTime.UtcNow,
                        Body = Encoding.UTF8.GetString(messageBody)  
                    };

                    _context.OutboxEvents.Add(outboxEvent);
                    await _context.SaveChangesAsync();

                    try
                    {
                        _publisher.Publish("order.created", messageBody);
                        outboxEvent.PublishedOn = DateTime.UtcNow; // Update publish time
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Message published successfully.");
                    }
                    catch (Exception publishEx)
                    {
                        // Logging the exception details
                        _logger.LogError(publishEx, "Failed to publish the message for Order ID {OrderId}.", order.Id);

                        // Writing to the console for demonstration (not recommended in production)
                        Console.WriteLine($"Failed to publish the message for Order ID {order.Id}. Exception: {publishEx.Message}");
                       
                        //Add a retry mechanism or flag the message for later processing.

                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Handle exceptions related to database operations
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        //public async Task CreateOrder(OrderModel order)
        //{
        //    _context.Orders.Add(order);

        //    var outboxEvent = new OutboxEventModel
        //    {
        //        EventId = Guid.NewGuid(),
        //        EventType = "OrderCreated",
        //        EventData = $"Order with ID {order.Id} created",
        //        CreatedOn = DateTime.UtcNow
        //    };

        //    _context.OutboxEvents.Add(outboxEvent);
        //    await _context.SaveChangesAsync();
        //    // Publish message
        //    var messageBody = CreateRabbitMqMessageBody(order); 
        //    _publisher.Publish("order.created", messageBody);
        //}

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
            var messageBody = CreateRabbitMqMessageBody(order);  
            _publisher.Publish("order.edited", messageBody);
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
                var messageBody = CreateRabbitMqMessageBody(order);
                _publisher.Publish("order.deleted", messageBody);
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
        public byte[] CreateRabbitMqMessageBody(OrderModel order)
        {
            if (order == null) return null;

            var body = Encoding.UTF8.GetBytes(string.Format("orderId={0}, orderAmount={1}", order.Id, order.TotalAmount));
            return body;
        }

    }
}
