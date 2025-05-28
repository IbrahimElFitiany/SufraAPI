
using Sufra.DTOs.OrderDTOS;
using Sufra.Models.Orders;

namespace Sufra.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task CancelOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> GetOrderDetailedByIdAsync(int orderId);
        Task<IEnumerable<Order>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO);
        Task<IEnumerable<Order>> GetRestaurantOrders(int restaurantId, OrderQueryDTO orderQuery);
        Task<IEnumerable<Order>> GetCustomerOrders(int customerId , OrderQueryDTO orderQuery);
        Task UpdateOrderAsync(Order order);
        Task SaveAsync();
    }
}
