using Sufra_MVC.Repositories;
using Sufra_MVC.Services.IServices;
using Sufra_MVC.Models.RestaurantModels;
using DTOs;
using Sufra_MVC.Models;

namespace Sufra_MVC.Services.Services
{
    public class DistrictServices : IDistrictServices
    {
        private readonly IDistrictRepository  _districtRepository;
        public DistrictServices(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public Task AddAsync(District district)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DistrictDTO>> GetAllAsync()
        {
            var districts = await _districtRepository.GetAllAsync();

            var districtDTOs = districts.Select(d => new DistrictDTO
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            return districtDTOs;
        }
        public Task<DistrictDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(District district)
        {
            throw new NotImplementedException();
        }

        //---------------------



    }
}
