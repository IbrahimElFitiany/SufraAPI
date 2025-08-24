using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.Common.Constants;
using Sufra.DTOs.MenuDTOs;
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemServices _menuItemServices;

        public MenuItemController(IMenuItemServices menuItemServices)
        {
            _menuItemServices = menuItemServices;
        }

        //-------------------------------------------------------------

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemReqDTO createMenuItemReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            MenuItemDTO menuItemDTO = new MenuItemDTO
            {
                RestaurantId = restaurantId,
                MenuSectionId = createMenuItemReqDTO.MenuSectionId,
                Name = createMenuItemReqDTO.Name,
                MenuItemImg = createMenuItemReqDTO.MenuItemImg,
                Description = createMenuItemReqDTO.Description,
                Price = createMenuItemReqDTO.Price,
                Availability = createMenuItemReqDTO.Availability
            };

            CreateMenuItemResDTO newMenuItem = await _menuItemServices.CreateMenuItemAsync(menuItemDTO);
            return Ok(newMenuItem);
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPut ("{menuItemId}")]
        public async Task<IActionResult> UpdateMenuItem([FromBody] CreateMenuItemReqDTO createMenuItemReqDTO , [FromRoute] int menuItemId) // Using createDTO for now, tight on time
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            MenuItemDTO menuItemDTO = new MenuItemDTO
            {
                MenuItemId = menuItemId,
                RestaurantId = restaurantId,
                MenuSectionId = createMenuItemReqDTO.MenuSectionId,
                Name = createMenuItemReqDTO.Name,
                MenuItemImg = createMenuItemReqDTO.MenuItemImg,
                Description = createMenuItemReqDTO.Description,
                Price = createMenuItemReqDTO.Price,
                Availability = createMenuItemReqDTO.Availability
            };

            await _menuItemServices.UpdateMenuItem(menuItemDTO);
            return Ok(new {message = "MenuItem Updated"});  
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpDelete("{menuItemId}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int menuItemId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            await _menuItemServices.RemoveMenuItemAsync(menuItemId,restaurantId);
            return Ok(new { message = "MenuItem deleted" });
        }

    }
}