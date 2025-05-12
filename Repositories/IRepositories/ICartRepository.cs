using Sufra_MVC.Models.Orders;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> CreateCartAsync(Cart cart);
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
        Task SaveAsync();
    }

}