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
            IEnumerable<CuisineDisplayDTO> cuisines = await _cuisineServices.GetAllWithImagesAsync();
            return Ok(cuisines);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCuisines()
        {
            IEnumerable<CuisineBasicDTO> cuisines = await _cuisineServices.GetAllAsync();
            return Ok(cuisines); 
        }

    }
}
