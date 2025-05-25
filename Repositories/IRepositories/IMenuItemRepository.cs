using SufraMVC.Models.Restaurants;

namespace SufraMVC.Repositories.IRepositories
{
    public interface IMenuItemRepository
    {
        Task CreateMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> GetMenuItemByRestaurantAndNameAsync(int restaurantId, string name);
        Task<MenuItem> GetMenuItemByIdAsync(int menuItemId);
        Task UpdateMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuItemAsync(MenuItem menuItem);
    }
}
