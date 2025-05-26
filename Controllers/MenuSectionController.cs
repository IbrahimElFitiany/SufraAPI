using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuSectionController : ControllerBase
    {
        private readonly IMenuSectionServices _menuSectionManagementServices;

        public MenuSectionController(IMenuSectionServices menuSectionManagementServices)
        {
            _menuSectionManagementServices = menuSectionManagementServices;
        }

        //------------------------------------------------

        [Authorize(Roles = "RestaurantManager")]
        [HttpPost]
        public async Task<IActionResult> CreateMenuSection ([FromBody] CreateMenuSectionReqDTO createMenuSectionReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                MenuSectionDTO menuSectionDTO = new MenuSectionDTO
                {
                    RestaurantId = restaurantId,
                    MenuSectionName = createMenuSectionReqDTO.MenuSectionName
                };

                CreateMenuSectionResDTO newMenuSection = await _menuSectionManagementServices.CreateAsync(menuSectionDTO);

                return Ok(newMenuSection);
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict (new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "RestaurantManager")]
        [HttpDelete("{menuSectionId}")]
        public async Task<IActionResult> DeleteMenuSection([FromRoute] int menuSectionId)
        {
            try
            {
                int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

                MenuSectionDTO menuSectionDTO = new MenuSectionDTO
                {
                    RestaurantId = restaurantId,
                    MenuSectionId = menuSectionId                };

                await _menuSectionManagementServices.DeleteAsync(menuSectionDTO);
                return Ok(new { message = "Deleted" });
            }
            catch (MenuSectionUnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (MenuSectionNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
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
        [HttpPatch ("{menuSectionId}")]
        public async Task<IActionResult> UpdateMenuSectionName([FromRoute] int menuSectionId , [FromBody] CreateMenuSectionReqDTO updateMenuSectionReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                MenuSectionDTO menuSection = new MenuSectionDTO
                {
                    MenuSectionId = menuSectionId,
                    RestaurantId = restaurantId,
                    MenuSectionName = updateMenuSectionReqDTO.MenuSectionName,
                };

                await _menuSectionManagementServices.UpdateAsync(menuSection);

                return Ok(new {message = "Updated the name"});
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
