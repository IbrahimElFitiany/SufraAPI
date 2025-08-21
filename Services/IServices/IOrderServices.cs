using Sufra.Common.Enums;
using Sufra.DTOs.OrderDTOS;

namespace Sufra.Services.IServices
{
    public interface IOrderServices
    {
       Task CreateOrderAsync(OrderDTO orderDTO);
       Task CancelOrderAsync(int orderId, int customerId);
       Task<OrderDetailedDTO> GetOrderAsync(int orderId, int customerId);
       Task<IEnumerable<OrderDTO>> GetRestaurantOrdersAsync(int restaurantId , OrderQueryDTO orderQuery);
       Task<OrderTrendDTO> GetRestaurantOrdersTrendAsync(int restaurantId, TrendPeriod period);
       Task<IEnumerable<OrderItemListDTO>> GetCustomerOrdersAsync(int customerId, OrderQueryDTO orderQuery);
       Task<IEnumerable<OrderDTO>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO);
       Task UpdateOrderStatus(OrderDTO orderDTO);
    }
}
