using DTOs;
using DTOs.MenuSectionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using Sufra_MVC.DTOs;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Services.IServices;

namespace Sufra_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        //-------------------------------------------------------------

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartReqDTO addToCartReqDTO)
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (CustomerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }

            try
            {
                await _cartServices.AddToCartAsync(addToCartReqDTO, CustomerId);
                return Ok(new { Message = "Item Added To cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
