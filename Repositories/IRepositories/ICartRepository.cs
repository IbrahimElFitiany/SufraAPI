using SufraMVC.Models.Orders;

namespace SufraMVC.Repositories.IRepositories
{
    public interface ICartRepository
    {
        Task<Cart> CreateCartAsync(Cart cart);
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
        Task DeleteCartAsync(Cart cart);
        Task SaveAsync();
    }

}