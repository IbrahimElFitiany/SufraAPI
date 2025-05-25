
using SufraMVC.DTOs;

namespace SufraMVC.Services.IServices
{
    public interface IOrderServices
    {
       Task CreateOrderAsync(OrderDTO orderDTO);
       Task CancelOrderAsync(int orderId, int customerId);
       Task<OrderDTO> GetOrder(int orderId, int customerId);
       Task<IEnumerable<OrderDTO>> GetRestaurantOrders(int restaurantId);
       Task<IEnumerable<OrderDTO>> GetCustomerOrders(int customerId);
        Task<IEnumerable<OrderDTO>> GetAllOrders();
       Task UpdateOrderStatus(OrderDTO orderDTO);
    }
}
