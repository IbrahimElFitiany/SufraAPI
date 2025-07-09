
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.CartDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;

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



        [Authorize (Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartReqDTO addToCartReqDTO)
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);

            try
            {
                await _cartServices.AddToCartAsync(addToCartReqDTO, CustomerId);
                return Ok(new { Message = "Item Added To cart" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MenuItemNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CartRestaurantConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int CustomerId = int.Parse(User.FindFirst("Id")?.Value);

            try
            {
                IEnumerable<CartListItemDTO> cartItems = await _cartServices.GetAllAsync(CustomerId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            try
            {
                await _cartServices.ClearCart(CustomerId);
                return Ok(new { Message = "Cart Cleared" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CartNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CartIsEmptyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int cartItemId)
        {
            int CustomerId = int.Parse(User.FindFirst("UserID")?.Value);
            try
            {
                await _cartServices.RemoveFromCartAsync(CustomerId , cartItemId);
                return Ok(new { Message = "Cart Item Deleted" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CartNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CartItemNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
