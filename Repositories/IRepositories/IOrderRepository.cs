
using Sufra.DTOs.OrderDTOS;
using Sufra.Models.Orders;

namespace Sufra.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task CancelOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO);
        Task<IEnumerable<Order>> GetRestaurantOrders(int restaurantId);
        Task<IEnumerable<Order>> GetCustomerOrders(int customerId);
        Task SaveAsync();
    }
}
