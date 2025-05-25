using Sufra.DTOs;

namespace Sufra.Services.IServices
{
    public interface ISearchServices
    {
        Task<IEnumerable<RestaurantDTO>> SearchRestaurantsAsync(string name);
    }
}
