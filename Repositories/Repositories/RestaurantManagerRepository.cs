using Microsoft.EntityFrameworkCore;
using Sufra_MVC.Data;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
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
            try
            {
                await _context.RestaurantManagers.AddAsync(manager);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Repo , AddManagerAsync: {e.Message}");
            }

        }

        public async Task<RestaurantManager> GetManagerByEmailAsync(string email)
        {
            try
            {
                RestaurantManager manager = await _context.RestaurantManagers.FirstOrDefaultAsync(m => m.Email == email);
                return manager;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<RestaurantManager> GetManagerByIdAsync(int id)
        {
            throw new NotImplementedException();
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
