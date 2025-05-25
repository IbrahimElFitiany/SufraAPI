using DTOs.MenuSectionDTOs;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Services.IServices
{
    public interface IMenuSectionServices
    {
        Task<CreateMenuSectionResDTO> CreateAsync(MenuSectionDTO menuSectionDTO);
        Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int id);
        Task DeleteAsync(MenuSectionDTO menuSectionDTO);
        Task UpdateByIdAsync(MenuSectionDTO menuSection);
    }
}
