
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs;
using Sufra.Services.IServices;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]

    public class SearchController : ControllerBase
    {
        private readonly ISearchServices _searchServices;
        public SearchController(ISearchServices searchServices)
        {
            _searchServices = searchServices;
        }

        //----------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> SearchRestaurantAsync ([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    //return BadRequest(new { message = "Search query cannot be empty."});
                }

                IEnumerable<RestaurantDTO> restaurants = await _searchServices.SearchRestaurantsAsync(q);
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest (new { message = ex.Message });
            }
        }

    }
}
