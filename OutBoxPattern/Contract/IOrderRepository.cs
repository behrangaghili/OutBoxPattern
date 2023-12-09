using OutBoxPattern.Models;

namespace OutBoxPattern.Contract
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int orderId);
        Task SaveChangesAsync();

        // Since your OrderService mentions updating order status,
        // you might also need a method specifically for that.
        Task UpdateOrderStatus(int orderId, string newStatus);
    }

}
