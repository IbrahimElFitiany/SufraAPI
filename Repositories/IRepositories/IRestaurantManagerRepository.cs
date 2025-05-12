using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
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
