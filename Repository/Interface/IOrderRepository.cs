using Models;

namespace Repository.Interface;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order, List<OrderDetail> orderDetails);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    Task<bool> CancelOrderAsync(int orderId);
    Task<List<Order>> GetOrders();
}