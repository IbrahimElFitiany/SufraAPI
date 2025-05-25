using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
{
    public interface ICuisineRepository
    {
        Task CreateAsync(Cuisine cuisine);
        Task<Cuisine> GetByIdAsync(int id);
        Task<IEnumerable<Cuisine>> GetAllAsync();
        Task UpdateAsync(Cuisine cuisine);
        Task DeleteAsync(int id);
    }

}