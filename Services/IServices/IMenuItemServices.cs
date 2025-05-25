using Sufra.DTOs;
using Sufra.DTOs.MenuSectionDTOs;

namespace Sufra.Services.IServices
{
    public interface IMenuItemServices
    {
        Task<CreateMenuItemResDTO> CreateMenuItemAsync(MenuItemDTO menuItemDTO);
        Task RemoveMenuItemAsync(int menuItemId, int restaurantId);
        Task UpdateMenuItem(MenuItemDTO menuItemDTO);
    }
}
