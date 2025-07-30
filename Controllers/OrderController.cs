
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Sufra.Common.Constants;
using Sufra.DTOs.OrderDTOS;
using Sufra.Exceptions;
using Sufra.Services.IServices;
using System.Security.Claims;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        //-------------------------------------------------------------

        [Authorize (Roles = RoleNames.Customer)]
        [HttpPost]
        public async Task<IActionResult> AddOrder()
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                OrderDTO order = new OrderDTO
                {
                    CustomerId = customerId,
                    OrderDate = DateTime.UtcNow
                };

               await _orderServices.CreateOrderAsync(order);
               return Ok(new { Message = "Order Created" });
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

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            try
            {
                OrderDetailedDTO order = await _orderServices.GetOrderAsync(orderId, customerId);
                return Ok(order);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetQueriedOrders([FromQuery] OrderQueryDTO orderQueryDTO)
        {
            try
            {
                IEnumerable<OrderDTO> orders =  await _orderServices.QueryOrdersAsync(orderQueryDTO);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet("customer-orders")]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] OrderQueryDTO orderQueryDTO)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var customerOrders = await _orderServices.GetCustomerOrdersAsync(customerId,orderQueryDTO);
                return Ok(customerOrders);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpGet("restaurant-orders")]
        public async Task<IActionResult> GetRestaurantOrder([FromQuery] OrderQueryDTO orderQuery)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                var restaurantOrders = await _orderServices.GetRestaurantOrdersAsync(restaurantId , orderQuery);
                return Ok(restaurantOrders);
            }
            catch(RestaurantNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [Authorize(Roles = RoleNames.Customer)]
        [HttpPatch ("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                await _orderServices.CancelOrderAsync(orderId ,customerId);
                return Ok(new { Message = "Order Canceled" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (OrderCancellationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]int orderId,UpdateOrderReqDTO updateOrderReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                OrderDTO orderDTO = new OrderDTO
                {
                    OrderId = orderId,
                    RestaurantId = restaurantId,
                    Status = updateOrderReqDTO.Status
                };

                await _orderServices.UpdateOrderStatus(orderDTO);
                return Ok(new {message = "updated" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Forbid($"Restaurant is not approved: {ex.Message}");
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (OrderUnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (OrderIsAlreadyCanceledException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
