
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs.OrderDTOS;
using Sufra.Services.IServices;

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOrder()
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (customerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }

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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (customerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            try
            {
                OrderDTO order = await _orderServices.GetOrderAsync(orderId, customerId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
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


        [Authorize]
        [HttpGet("customer-orders")]
        public async Task<IActionResult> GetCustomerOrders()
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (customerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            try
            {
                var customerOrders = await _orderServices.GetCustomerOrdersAsync(customerId);
                return Ok(customerOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpGet("restaurant-orders")]
        public async Task<IActionResult> GetRestaurantOrder()
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            if (restaurantId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            try
            {
                var restaurantOrders = await _orderServices.GetRestaurantOrdersAsync(restaurantId);
                return Ok(restaurantOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "'test'" });
            }
        }



        [Authorize]
        [HttpPatch ("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);
            if (customerId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            try
            {
                await _orderServices.CancelOrderAsync(orderId ,customerId);
                return Ok(new { Message = "Order Canceled" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute]int orderId,UpdateOrderReqDTO updateOrderReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            if (restaurantId == null)
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
