using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
{
    public interface IRestaurantRepository
    {
        Task CreateAsync(Restaurant restaurant);
        Task<Restaurant> GetByIdAsync(int id);
        Task<Restaurant> GetByNameAsync(string RestaurantName);
        Task<Restaurant> GetByManagerIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<IEnumerable<Restaurant>> GetSufraPicksAsync();
        Task UpdateRestaurant(Restaurant restaurant);
        Task ApproveRestaurantById(Restaurant restaurant);
        Task BlockRestaurantById(Restaurant restaurant);
        Task DeleteRestaurant(Restaurant restaurant);
        Task<bool?> GetRestaurantStatusByIdAsync(int restaurantId);
        Task<bool> ExistsAsync(int restaurantId);
        Task SaveAsync();
    }
}
