using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
{
    public interface IRestaurantManagerRepository
    {
        Task AddManagerAsync(RestaurantManager manager);
        Task<RestaurantManager> GetManagerByIdAsync(int id);
        Task<RestaurantManager> GetManagerByEmailAsync(string email);
        Task UpdateManagerAsync(RestaurantManager manager);
        Task DeleteManagerAsync(int id);
        Task<List<RestaurantManager>> GetAllManagersAsync();
    }
}
