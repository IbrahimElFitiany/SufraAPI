using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.Common.Constants;
using Sufra.DTOs.CartDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;
using System.Security.Claims;

namespace Sufra.Controllers
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


        [Authorize (Roles = RoleNames.Customer)]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartReqDTO addToCartReqDTO)
        {
            int CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            await _cartServices.AddToCartAsync(addToCartReqDTO, CustomerId);
            return Ok(new { Message = "Item Added To cart" });
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            IEnumerable<CartListItemDTO> cartItems = await _cartServices.GetAllAsync(CustomerId);
            return Ok(cartItems);
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            int CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            await _cartServices.ClearCart(CustomerId);
            return Ok(new { Message = "Cart Cleared" });
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int cartItemId)
        {
            int CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            await _cartServices.RemoveFromCartAsync(CustomerId , cartItemId);
            return Ok(new { Message = "Cart Item Deleted" });
        }

    }
}
