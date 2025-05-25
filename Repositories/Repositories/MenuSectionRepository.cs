using Microsoft.EntityFrameworkCore;
using SufraMVC.Data;
using SufraMVC.Models.Restaurants;
using SufraMVC.Repositories.IRepositories;

namespace SufraMVC.Repositories.Repositories
{
    public class MenuSectionRepository : IMenuSectionRepository
    {
        private readonly Sufra_DbContext _context;
        
        public MenuSectionRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //-----------------------------------------------------

        public async Task<MenuSection> CreateAsync(MenuSection menuSection)
        {
                await _context.MenuSections.AddAsync(menuSection);
                await _context.SaveChangesAsync();

                return menuSection;
        }

        public async Task DeleteAsync(MenuSection menuSection)
        {
            _context.MenuSections.Remove(menuSection);
            await _context.SaveChangesAsync();
        }

        public async Task<MenuSection?> GetByIdAsync(int id)
        {
            MenuSection menuSection = await _context.MenuSections.FirstOrDefaultAsync(ms => ms.Id == id);
            return menuSection;
        }

        public Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(MenuSection menuSection)
        {
            _context.MenuSections.Update(menuSection);
            await _context.SaveChangesAsync();
        }
    }
}
