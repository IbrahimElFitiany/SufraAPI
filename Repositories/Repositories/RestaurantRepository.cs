using DuoVia.FuzzyStrings;
using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly Sufra_DbContext _context;

        public RestaurantRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //-----------------------------

        public async Task CreateAsync(Restaurant restaurant)
        {
                await _context.Restaurants.AddAsync(restaurant);
                await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Restaurant>> QueryRestaurantsAsync(RestaurantQueryDTO dto)
        {
         
            IQueryable<Restaurant> query = _context.Restaurants;

            if (dto.IsApproved.HasValue)
            {
                query = query.Where(r => r.IsApproved == dto.IsApproved);
            }

            if (string.IsNullOrEmpty(dto.NormalizedQuery))
            {
                return await query
                    .Skip((dto.Page - 1) * dto.PageSize)
                    .Take(dto.PageSize)
                    .ToListAsync();
            }

            // Fuzzy matching must be done in-memory
            List<Restaurant> allFiltered = await query.ToListAsync();

            var fuzzyFiltered = allFiltered.Where(r =>
                (r.Name != null && r.Name.FuzzyMatch(dto.NormalizedQuery) >= 0.2) ||
                (r.District?.Name != null && r.District.Name.FuzzyMatch(dto.NormalizedQuery) >= 0.2) ||
                (r.Cuisine?.Name != null && r.Cuisine.Name.FuzzyMatch(dto.NormalizedQuery) >= 0.2)
            ).ToList();

            return fuzzyFiltered
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToList();
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



        public async Task ApproveRestaurant(Restaurant restaurant)
        {
            restaurant.IsApproved = true;
            await _context.SaveChangesAsync();
        }
        public async Task BlockRestaurant(Restaurant restaurant ) {
            restaurant.IsApproved = false;
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
