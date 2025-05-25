using Sufra.Repositories.IRepositories;
using Sufra.DTOs;
using Sufra.Services.IServices;
using Sufra.Models.Restaurants;

namespace Sufra.Services.Services
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

        public async Task<IEnumerable<CuisineDTO>> GetAllAsync()
        {
            var cuisines = await _cuisineRepository.GetAllAsync();  // Fetch cuisines from repository

            var cuisineDTOs = cuisines.Select(c => new CuisineDTO
            {
                CuisineId = c.Id,
                CuisineName = c.Name
            }).ToList();

            return cuisineDTOs;
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
