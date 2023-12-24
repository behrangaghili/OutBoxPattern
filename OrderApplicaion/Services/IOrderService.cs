using OrderMicroservice.Models;

namespace OrderMicroservice.Services
{
    public interface IOrderService
    {
        Task CreateOrder(OrderModel order);
        Task DeleteOrder(int orderId);
        Task<List<OrderModel>> GetAllOrders();
        Task<OrderModel> GetOrderById(int orderId);
    }
}