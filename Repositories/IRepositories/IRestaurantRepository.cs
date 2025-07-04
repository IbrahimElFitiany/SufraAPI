using Sufra.Common.Models;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
{
    public interface IRestaurantRepository
    {
        Task CreateAsync(Restaurant restaurant);
        Task<Restaurant> GetByIdAsync(int id);
        Task<Restaurant> GetByNameAsync(string RestaurantName);
        Task<Restaurant> GetByManagerIdAsync(int id);
        Task<PagedQueryResult<Restaurant>> QueryRestaurantsAsync(RestaurantQueryDTO query);
        Task<IEnumerable<Restaurant>> GetSufraPicksAsync();
        Task UpdateRestaurant(Restaurant restaurant);
        Task ApproveRestaurant(Restaurant restaurant);
        Task BlockRestaurant(Restaurant restaurant);
        Task DeleteRestaurant(Restaurant restaurant);
        Task<bool?> GetRestaurantStatusByIdAsync(int restaurantId);
        Task<bool> ExistsAsync(int restaurantId);
        Task SaveAsync();
    }
}
