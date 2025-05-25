using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public interface IMenuSectionRepository
    {
        Task<MenuSection> CreateAsync(MenuSection menuSection);
        Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int restaurantId);
        Task<MenuSection?> GetByIdAsync(int id);
        Task DeleteAsync(MenuSection menuSection);
        Task UpdateAsync(MenuSection menuSection);
    }
}
