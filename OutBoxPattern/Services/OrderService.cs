using System;
using OutBoxPattern.Models;
using System.Threading.Tasks;
using OutBoxPattern.Persistence.OutBoxPattern.Data;
using OutBoxPattern.Contract;

namespace OutBoxPattern.Services
{
    public class OrderService : IOrderService
    {
        private readonly OutBoxContext _context;

        public OrderService(OutBoxContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        
    }

    
}

