using DispatcherService.Models;

namespace DispatcherService.Contract
{
    public interface IOrderService
    {
        Task CreateOrder(OrderModel order);
        Task DeleteOrder(int orderId);
        Task<OrderModel>GetOrderById(int orderId);
        Task<List<OrderModel>> GetAllOrders();
    }
}
