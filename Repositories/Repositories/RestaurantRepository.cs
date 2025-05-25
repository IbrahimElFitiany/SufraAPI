using Microsoft.EntityFrameworkCore;
using Sufra_MVC.Data;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly Sufra_DbContext _context;

        public RestaurantRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //---------------

        public async Task CreateAsync(Restaurant restaurant)
        {
                await _context.Restaurants.AddAsync(restaurant);
                await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetSufraPicksAsync()
        {
            return await _context.Restaurants
                .OrderByDescending(r => r.Rating)
                .Take(4)
                .ToListAsync();
        }

        public async Task<Restaurant> GetByIdAsync(int id)
        {
             return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Restaurant> GetByManagerIdAsync(int managerId)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.ManagerId == managerId);
        }
        public async Task<Restaurant> GetByNameAsync(string RestaurantName)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == RestaurantName);
        }
        public async Task<bool?> GetRestaurantStatusByIdAsync(int restaurantId)
        {
            bool exists = await _context.Restaurants.AnyAsync(r => r.Id == restaurantId);
            if (!exists) return null;

            bool IsApproved = await _context.Restaurants
                .Where(r => r.Id == restaurantId)
                .Select(r => r.IsApproved)
                .FirstOrDefaultAsync();

            return IsApproved;
        }


        public async Task ApproveRestaurantById(Restaurant restaurant)
        {
            restaurant.IsApproved = true;
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task BlockRestaurantById(Restaurant restaurant ) {
            restaurant.IsApproved = false;
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.Id == restaurantId);
        }
    }
}
