using SufraMVC.DTOs.MenuSectionDTOs;
using SufraMVC.Models.Restaurants;

namespace SufraMVC.Services.IServices
{
    public interface IMenuSectionServices
    {
        Task<CreateMenuSectionResDTO> CreateAsync(MenuSectionDTO menuSectionDTO);
        Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int id);
        Task DeleteAsync(MenuSectionDTO menuSectionDTO);
        Task UpdateByIdAsync(MenuSectionDTO menuSection);
    }
}
