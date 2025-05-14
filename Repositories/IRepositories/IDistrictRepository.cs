
using Sufra_MVC.Models;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
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