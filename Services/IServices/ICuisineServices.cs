using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Services.IServices
{
    public interface ICuisineServices
    {
        Task AddAsync(Cuisine cuisine);
        Task<Cuisine> GetByIdAsync(int id);
        Task<IEnumerable<Cuisine>> GetAllAsync();
        Task UpdateAsync(Cuisine cuisine);
        Task DeleteAsync(int id);

    }
}
