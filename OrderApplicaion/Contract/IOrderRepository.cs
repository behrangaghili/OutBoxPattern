using OrderApplicaion.Models;

namespace OrderApplicaion.Contract
{
    public interface IOrderRepository
    {
        Task<OrderModel> GetByIdAsync(int orderId);
        Task<IEnumerable<OrderModel>> GetAllAsync();
        Task AddAsync(OrderModel order);
        Task UpdateAsync(OrderModel order);
        Task DeleteAsync(int orderId);
        Task SaveChangesAsync();

        // Since your OrderService mentions updating order status,
        // you might also need a method specifically for that.
        Task UpdateOrderStatus(int orderId, string newStatus);
    }

}
