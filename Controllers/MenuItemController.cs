using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "RestaurantManager")]
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
            catch (MenuSectionNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuSectionUnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (MenuItemAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "RestaurantManager")]
        [HttpPut ("{menuItemId}")]
        public async Task<IActionResult> UpdateMenuItem([FromBody] CreateMenuItemReqDTO createMenuItemReqDTO , [FromRoute] int menuItemId) // Using createDTO for now, tight on time
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
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
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuSectionNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuSectionUnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (MenuItemNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuItemUnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "RestaurantManager")]
        [HttpDelete("{menuItemId}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int menuItemId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _menuItemServices.RemoveMenuItemAsync(menuItemId,restaurantId);
                return Ok(new { message = "MenuItem deleted" });
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuItemNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuItemUnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
