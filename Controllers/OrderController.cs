using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Orders;
using Services.IServices;
using Sufra_MVC.Services.IServices;

namespace Sufra_MVC.Controllers
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
               // await _orderServices.CreateOrderByCustomeridAsync(customerId);
                return Ok(new { Message = "Order Created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
