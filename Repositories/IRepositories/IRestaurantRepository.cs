
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public interface IRestaurantRepository
    {
        Task CreateAsync(Restaurant restaurant);
        Task<Restaurant> GetByIdAsync(int id);
        Task<Restaurant> GetByNameAsync(string RestaurantName);
        Task<Restaurant> GetByManagerIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task UpdateRestaurant(Restaurant restaurant);
        Task ApproveRestaurantById(Restaurant restaurant);
        Task BlockRestaurantById(Restaurant restaurant);
        Task DeleteRestaurant(int id);
        Task<bool?> GetRestaurantStatusByIdAsync(int restaurantId);
        Task<bool> ExistsAsync(int restaurantId);
        Task SaveAsync();
    }
}
