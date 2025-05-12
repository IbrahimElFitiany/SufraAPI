using Microsoft.EntityFrameworkCore;
using Sufra_MVC.Data;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public class CuisineRepository : ICuisineRepository
    {
        private readonly Sufra_DbContext _context;

        public CuisineRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------

        public async Task CreateAsync(Cuisine cuisine)
        {
            try
            {
                await _context.Cuisines.AddAsync(cuisine);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error In Repo Layer: {ex.Message}");
            }
        }
        public async Task<Cuisine> GetByIdAsync(int id)
        {
            try
            {
                Cuisine cuisine = await _context.Cuisines.FirstOrDefaultAsync(c => c.Id == id);
                return cuisine;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error In Repo Layer: {ex.Message}");
            }
        }
        public async Task UpdateAsync(Cuisine cuisine)
        {
            try
            {
                var existingCuisine = await _context.Cuisines.FirstOrDefaultAsync(c => c.Id == cuisine.Id);

                if (existingCuisine == null)
                {
                    throw new Exception("Cuisine not found.");
                }

                existingCuisine.Name = cuisine.Name;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error In Repo Layer: {ex.Message}");
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var cuisine = await _context.Cuisines.FirstOrDefaultAsync(c => c.Id == id);

                if (cuisine == null)
                {
                    throw new Exception("Cuisine not found.");
                }

                _context.Cuisines.Remove(cuisine);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error In Repo Layer: {ex.Message}");
            }
        }

    }
}
