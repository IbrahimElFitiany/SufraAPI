using SufraMVC.Models.Restaurants;

namespace SufraMVC.Repositories.IRepositories
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