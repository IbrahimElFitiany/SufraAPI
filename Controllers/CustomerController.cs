using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.CustomerDTOs;
using Sufra.Services.IServices;
using System.Threading.Tasks;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;

        public CustomerController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        //----------------------------------------------

        [HttpPost("login")]
        public async Task<IActionResult> Login( [FromBody] LoginDTO loginDTO)
        {
            try
            {
                LoginResponseDTO loginResponseDTO = await _customerServices.LoginAsync(loginDTO);
                return Ok(loginResponseDTO);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                RegisterResponseDTO createCustomer = await _customerServices.RegisterAsync(registerDTO);
                return Ok(createCustomer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
