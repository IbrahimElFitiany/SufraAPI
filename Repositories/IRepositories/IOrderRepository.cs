using Models.Orders;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;

namespace Sufra_MVC.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Cart> GetCartByCustomer(Customer customer);
        Task SaveAsync();
    }
}
