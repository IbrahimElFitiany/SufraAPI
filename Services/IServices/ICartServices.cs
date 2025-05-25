using SufraMVC.DTOs;

namespace SufraMVC.Services.IServices
{
    public interface ICartServices
    {
        Task AddToCartAsync(AddToCartReqDTO addToCartReqDTO, int customerId);
        Task ClearCart(int customerId);
        Task<IEnumerable<GetCartItemReqDTO>> GetAllAsync(int customerId);
        Task RemoveFromCartAsync(int customerId, int cartItemId);
    }
}