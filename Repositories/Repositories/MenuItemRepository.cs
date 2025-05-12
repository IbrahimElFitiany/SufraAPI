using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Sufra_MVC.Data;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
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
            try
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            throw new NotImplementedException();
        }
    }
}
