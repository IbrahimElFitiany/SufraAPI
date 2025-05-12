
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public interface ICuisineRepository
    {
        Task CreateAsync(Cuisine cuisine);
        Task<Cuisine> GetByIdAsync(int id);
        Task UpdateAsync(Cuisine cuisine);
        Task DeleteAsync(int id);
    }

}