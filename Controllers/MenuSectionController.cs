using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SufraMVC.DTOs.MenuSectionDTOs;
using SufraMVC.Exceptions;
using SufraMVC.Services.IServices;

namespace SufraMVC.Controllers
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

        [Authorize]
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

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteMenuSection([FromBody] DeleteMenuSectionReqDTO deleteMenuSectionReqDTO)
        {
            try
            {
                int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

                MenuSectionDTO menuSectionDTO = new MenuSectionDTO
                {
                    RestaurantId = restaurantId,
                    MenuSectionId = deleteMenuSectionReqDTO.MenusectionId
                };

                await _menuSectionManagementServices.DeleteAsync(menuSectionDTO);
                return Ok(new { message = "Deleted" });
            }
            catch (UnauthorizedAccessException ex)
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

        [Authorize]
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

                await _menuSectionManagementServices.UpdateByIdAsync(menuSection);

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
