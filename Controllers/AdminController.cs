
using Microsoft.AspNetCore.Mvc;
using SufraMVC.DTOs;
using SufraMVC.Services.IServices;

namespace SufraMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        private readonly IRestaurantServices _restaurantServices;
        public AdminController(IAdminServices adminServices , IRestaurantServices restaurantServices)
        {
            _adminServices = adminServices;
            _restaurantServices = restaurantServices;
        }

        //---------------------

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequestDTO adminLoginRequestDTO)
        {
            try
            {
                AdminLoginResponseDTO adminLoginResponseDTO = await _adminServices.Login(adminLoginRequestDTO);
                return Ok(adminLoginResponseDTO);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
