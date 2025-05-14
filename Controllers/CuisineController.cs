using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra_MVC.DTOs;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Services.IServices;

namespace sufra.Sufra.Emps.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineController : ControllerBase
    {
        private readonly ICuisineServices _cuisineServices;
        public CuisineController(ICuisineServices cuisineServices)
        {
            _cuisineServices = cuisineServices;
        }

        //---------------------

        [HttpGet]
        public async Task<IActionResult> GetCuisines()
        {
            try
            {
                IEnumerable<CuisineDTO> cuisines = await _cuisineServices.GetAllAsync();
                return Ok(cuisines);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
