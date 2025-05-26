using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Models.Restaurants;

namespace Sufra.Services.IServices
{
    public interface IMenuSectionServices
    {
        Task<CreateMenuSectionResDTO> CreateAsync(MenuSectionDTO menuSectionDTO);
        Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int id);
        Task DeleteAsync(MenuSectionDTO menuSectionDTO);
        Task UpdateAsync(MenuSectionDTO menuSection);
    }
}
