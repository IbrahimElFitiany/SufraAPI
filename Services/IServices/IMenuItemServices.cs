using DTOs.MenuSectionDTOs;
using Sufra_MVC.DTOs;

namespace Sufra_MVC.Services.IServices
{
    public interface IMenuItemServices
    {
        Task<CreateMenuItemResDTO> CreateMenuItemAsync(MenuItemDTO menuItemDTO);
        Task RemoveMenuItemAsync(int menuItemId, int restaurantId);
        Task UpdateMenuItem(MenuItemDTO menuItemDTO);
    }
}
