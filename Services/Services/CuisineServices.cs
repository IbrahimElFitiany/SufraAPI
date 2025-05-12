using Sufra_MVC.Repositories;
using Sufra_MVC.Services.IServices;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Services.Services
{
    public class CuisineServices: ICuisineServices
    {
        private readonly ICuisineRepository _cuisineRepository;
        public CuisineServices(ICuisineRepository cuisineRepository)
        {
            _cuisineRepository = cuisineRepository;
        }

        //---------------------
        public Task AddAsync(Cuisine cuisine)
        {

            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cuisine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cuisine> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cuisine cuisine)
        {
            throw new NotImplementedException();
        }

        //----------------------------


    }
}
