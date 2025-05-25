using SufraMVC.DTOs;
using SufraMVC.DTOs.MenuSectionDTOs;

namespace SufraMVC.Services.IServices
{
    public interface IMenuItemServices
    {
        Task<CreateMenuItemResDTO> CreateMenuItemAsync(MenuItemDTO menuItemDTO);
        Task RemoveMenuItemAsync(int menuItemId, int restaurantId);
        Task UpdateMenuItem(MenuItemDTO menuItemDTO);
    }
}
