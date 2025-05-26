using Sufra.DTOs.CartDTOs;

namespace Sufra.Services.IServices
{
    public interface ICartServices
    {
        Task AddToCartAsync(AddToCartReqDTO addToCartReqDTO, int customerId);
        Task ClearCart(int customerId);
        Task<IEnumerable<CartItemResponseDTO>> GetAllAsync(int customerId);
        Task RemoveFromCartAsync(int customerId, int cartItemId);
    }
}