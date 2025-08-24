using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.Common.Constants;
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPost]
        public async Task<IActionResult> CreateMenuSection ([FromBody] CreateMenuSectionReqDTO createMenuSectionReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            MenuSectionDTO menuSectionDTO = new MenuSectionDTO{
                RestaurantId = restaurantId,
                MenuSectionName = createMenuSectionReqDTO.MenuSectionName
            };

            CreateMenuSectionResDTO newMenuSection = await _menuSectionManagementServices.CreateAsync(menuSectionDTO);

            return Ok(newMenuSection);
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpDelete("{menuSectionId}")]
        public async Task<IActionResult> DeleteMenuSection([FromRoute] int menuSectionId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            MenuSectionDTO menuSectionDTO = new MenuSectionDTO
            {
                RestaurantId = restaurantId,
                MenuSectionId = menuSectionId                
            };

            await _menuSectionManagementServices.DeleteAsync(menuSectionDTO);
            return Ok(new { message = "Deleted" });
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPatch ("{menuSectionId}")]
        public async Task<IActionResult> UpdateMenuSectionName([FromRoute] int menuSectionId , [FromBody] CreateMenuSectionReqDTO updateMenuSectionReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            MenuSectionDTO menuSection = new MenuSectionDTO
            {
                MenuSectionId = menuSectionId,
                RestaurantId = restaurantId,
                MenuSectionName = updateMenuSectionReqDTO.MenuSectionName,
            };

            await _menuSectionManagementServices.UpdateAsync(menuSection);

            return Ok(new {message = "Updated the name"});
        }

    }
}