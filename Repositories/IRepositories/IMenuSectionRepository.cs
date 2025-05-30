﻿using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
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
