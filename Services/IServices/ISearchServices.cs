using SufraMVC.DTOs;

namespace SufraMVC.Services.IServices
{
    public interface ISearchServices
    {
        Task<IEnumerable<RestaurantDTO>> SearchRestaurantsAsync(string name);
    }
}
