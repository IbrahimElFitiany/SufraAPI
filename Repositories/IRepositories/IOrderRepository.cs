using Models.Orders;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task CancelOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetRestaurantOrders(int restaurantId);
        Task<IEnumerable<Order>> GetCustomerOrders(int customerId);
        Task SaveAsync();
    }
}
