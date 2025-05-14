using DTOs.MenuSectionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra_MVC.DTOs;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Services.IServices;

namespace Sufra_MVC.Controllers
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemReqDTO createMenuItemReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{menuItemId}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int menuItemId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _menuItemServices.RemoveMenuItemAsync(menuItemId,restaurantId);
                return Ok(new { message = "MenuItem deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }



    }
}
