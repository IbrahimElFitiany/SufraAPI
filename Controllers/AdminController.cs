using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sufra_MVC.DTOs;
using Sufra_MVC.Services.IServices;

namespace sufra.Sufra.Emps.Presentation.Controllers
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
