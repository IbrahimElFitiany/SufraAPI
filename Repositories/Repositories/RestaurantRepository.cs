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
            await _context.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS pg_trgm");

            IQueryable<Restaurant> query = _context.Restaurants.Include(r => r.Cuisine).Include(r => r.District);

            if (dto.IsApproved.HasValue) query = query.Where(r => r.IsApproved == dto.IsApproved);
            if (dto.DistrictId.HasValue) query = query.Where(r => r.DistrictId ==  dto.DistrictId);
            if (dto.CuisineId.HasValue) query = query.Where(r => r.CuisineId == dto.CuisineId);


            if (!string.IsNullOrEmpty(dto.NormalizedQuery))
            {
                var fuzzyQuery = query
                    .Where(r =>
                        EF.Functions.TrigramsSimilarity(r.Name.Replace(" ", ""), dto.NormalizedQuery) > 0.3 ||
                        EF.Functions.TrigramsSimilarity(r.District.Name.Replace(" ", ""), dto.NormalizedQuery) > 0.3 ||
                        EF.Functions.TrigramsSimilarity(r.Cuisine.Name.Replace(" ", ""), dto.NormalizedQuery) > 0.3)
                    .OrderByDescending(r =>
                        EF.Functions.TrigramsSimilarity(r.Name, dto.NormalizedQuery) +
                        EF.Functions.TrigramsSimilarity(r.District.Name, dto.NormalizedQuery) +
                        EF.Functions.TrigramsSimilarity(r.Cuisine.Name, dto.NormalizedQuery))
                    .Skip((dto.Page - 1) * dto.PageSize)
                    .Take(dto.PageSize);

                return await fuzzyQuery.ToListAsync();
            }

            return await query
            .OrderBy(r => r.Id)
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .ToListAsync();
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
