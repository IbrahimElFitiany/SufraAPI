
using Sufra.DTOs;
using Sufra.Models;

namespace Sufra.Services.IServices
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
