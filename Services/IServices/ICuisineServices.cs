using Sufra.DTOs.CuisineDTOs;
using Sufra.Models.Restaurants;

namespace Sufra.Services.IServices
{
    public interface ICuisineServices
    {
        Task AddAsync(Cuisine cuisine);
        Task<Cuisine> GetByIdAsync(int id);
        Task<IEnumerable<CuisineDisplayDTO>> GetAllWithImagesAsync();
        Task<IEnumerable<CuisineBasicDTO>> GetAllAsync();
        Task UpdateAsync(Cuisine cuisine);
        Task DeleteAsync(int id);

    }
}
