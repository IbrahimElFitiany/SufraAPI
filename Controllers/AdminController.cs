
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.SufraEmpDTOs;
using Sufra.Services.IServices;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
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
