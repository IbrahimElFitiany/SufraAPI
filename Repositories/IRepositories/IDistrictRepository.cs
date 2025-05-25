using SufraMVC.Models;

namespace SufraMVC.Repositories.IRepositories
{
    public interface IDistrictRepository
    {
        Task CreateAsync(District cuisine);
        Task<District> GetByIdAsync(int id);
        Task<IEnumerable<District>> GetAllAsync();
        Task UpdateAsync(District cuisine);
        Task DeleteAsync(int id);
    }

}