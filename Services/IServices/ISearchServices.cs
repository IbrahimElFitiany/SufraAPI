using Sufra_MVC.DTOs;

namespace Services.IServices
{
    public interface ISearchServices
    {
        Task<IEnumerable<RestaurantDTO>> SearchRestaurantsAsync(string name);
    }
}
