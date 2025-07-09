using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class RestaurantManagerRepository : IRestaurantManagerRepository
    {

        private readonly Sufra_DbContext _context;
        public RestaurantManagerRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //--------------------------------------

        public async Task AddManagerAsync(RestaurantManager manager)
        {
            await _context.RestaurantManagers.AddAsync(manager);
            await _context.SaveChangesAsync();
        }

        public async Task<RestaurantManager> GetManagerByEmailAsync(string email)
        {
            RestaurantManager manager = await _context.RestaurantManagers.FirstOrDefaultAsync(m => m.Email == email);
            return manager;
        }

        public async Task<RestaurantManager> GetManagerByIdAsync(int id)
        {
            return await _context.RestaurantManagers.FirstOrDefaultAsync(rm => rm.Id == id);
        }

        public Task<List<RestaurantManager>> GetAllManagersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateManagerAsync(RestaurantManager manager)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManagerAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
