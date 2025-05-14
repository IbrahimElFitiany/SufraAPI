using DTOs;
using Sufra_MVC.Models;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Services.IServices
{
    public interface IDistrictServices
    {
        Task AddAsync(District district);
        Task<DistrictDTO> GetByIdAsync(int id);
        Task<IEnumerable<DistrictDTO>> GetAllAsync();
        Task UpdateAsync(District district);
        Task DeleteAsync(int id);

    }
}
