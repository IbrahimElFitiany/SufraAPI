using Sufra.DTOs.OrderDTOS;

namespace Sufra.Services.IServices
{
    public interface IOrderServices
    {
       Task CreateOrderAsync(OrderDTO orderDTO);
       Task CancelOrderAsync(int orderId, int customerId);
       Task<OrderDTO> GetOrderAsync(int orderId, int customerId);
       Task<IEnumerable<OrderDTO>> GetRestaurantOrdersAsync(int restaurantId);
       Task<IEnumerable<OrderDTO>> GetCustomerOrdersAsync(int customerId);
        Task<IEnumerable<OrderDTO>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO);
       Task UpdateOrderStatus(OrderDTO orderDTO);
    }
}
