using SufraMVC.DTOs;
using SufraMVC.Models.Restaurants;

namespace SufraMVC.Services.IServices
{
    public interface ICuisineServices
    {
        Task AddAsync(Cuisine cuisine);
        Task<Cuisine> GetByIdAsync(int id);
        Task<IEnumerable<CuisineDTO>> GetAllAsync();
        Task UpdateAsync(Cuisine cuisine);
        Task DeleteAsync(int id);

    }
}
