using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.CustomerDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;

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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDTO registerDTO)
        {
            RegisterResponseDTO createCustomer = await _customerServices.RegisterAsync(registerDTO);
            return Ok(createCustomer);
        }
    }
}
