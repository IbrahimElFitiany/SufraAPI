using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.CustomerDTOs;
using Sufra.Exceptions;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login( [FromBody] CustomerLoginReqDTO loginDTO)
        {
            try
            {
                CustomerLoginResDTO loginResponseDTO = await _customerServices.LoginAsync(loginDTO);
                return Ok(loginResponseDTO);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDTO registerDTO)
        {
            try
            {
                RegisterResponseDTO createCustomer = await _customerServices.RegisterAsync(registerDTO);
                return Ok(createCustomer);
            }
            catch (EmailAlreadyInUseException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (PhoneAlreadyInUseException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
