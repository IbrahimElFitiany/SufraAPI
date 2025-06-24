using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.CuisineDTOs;
using Sufra.Services.IServices;

namespace Sufra.Controllers
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

        [AllowAnonymous]
        [HttpGet("with-images")]
        public async Task<IActionResult> GetCuisinesWithImages()
        {
            try
            {
                IEnumerable<CuisineDisplayDTO> cuisines = await _cuisineServices.GetAllWithImagesAsync();
                return Ok(cuisines);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCuisines()
        {
            try
            {
                IEnumerable<CuisineBasicDTO> cuisines = await _cuisineServices.GetAllAsync();
                return Ok(cuisines);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
