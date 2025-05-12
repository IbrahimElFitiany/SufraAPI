using DTOs;

namespace Services.IServices
{
    public interface ICartServices
    {
        Task AddToCartAsync(AddToCartReqDTO addToCartReqDTO, int customerId);  
    }
}