﻿using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class MenuItemRepository:IMenuItemRepository
    {
        private readonly Sufra_DbContext _context;

        public MenuItemRepository (Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------------------------------------

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }
        public async Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
        {
            return await _context.MenuItems.FindAsync(menuItemId);
        }
        public async Task<MenuItem> GetMenuItemByRestaurantAndNameAsync(int restaurantId, string name)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(m => m.RestaurantId == restaurantId && m.Name == name);
        }
        public async Task DeleteMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
