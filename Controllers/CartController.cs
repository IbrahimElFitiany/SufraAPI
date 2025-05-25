
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SufraMVC.DTOs;
using SufraMVC.Services.IServices;

namespace SufraMVC.Controllers
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (CustomerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }

            try
            {
                IEnumerable<GetCartItemReqDTO> cartItems = await _cartServices.GetAllAsync(CustomerId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (CustomerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }

            try
            {
                await _cartServices.ClearCart(CustomerId);
                return Ok(new { Message = "Cart Cleared" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int cartItemId)
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (CustomerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }

            try
            {
                await _cartServices.RemoveFromCartAsync(CustomerId , cartItemId);
                return Ok(new { Message = "Cart Item Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
