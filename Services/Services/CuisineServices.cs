using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;
using Sufra.Models.Restaurants;
using Sufra.DTOs.CuisineDTOs;

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

        public async Task<IEnumerable<CuisineDisplayDTO>> GetAllWithImagesAsync()
        {
            var cuisines = await _cuisineRepository.GetAllAsync();

            var cuisineDisplayDTOs = cuisines.Select(c => new CuisineDisplayDTO
            {
                CuisineId = c.Id,
                CuisineName = c.Name,
                CuisineImage = c.CuisineImage
            }).ToList();

            return cuisineDisplayDTOs;
        }

        public async Task<IEnumerable<CuisineBasicDTO>> GetAllAsync()
        {
            var cuisines = await _cuisineRepository.GetAllAsync();

            var cuisineBasicDTOs = cuisines.Select(c => new CuisineBasicDTO
            {
                CuisineId = c.Id,
                CuisineName = c.Name,
            }).ToList();

            return cuisineBasicDTOs;
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
