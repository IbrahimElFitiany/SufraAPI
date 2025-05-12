using DTOs.MenuSectionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra_MVC.Exceptions;
using Sufra_MVC.Services.IServices;

namespace sufra.Controllers
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


    }
}
