using DTOs;
using DTOs.TableDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra_MVC.DTOs;
using Sufra_MVC.Exceptions;
using Sufra_MVC.Services.IServices;

namespace sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {

        private readonly IRestaurantServices _restaurantServices;

        public RestaurantController(IRestaurantServices restaurantServices)
        {
            _restaurantServices = restaurantServices;
        }

        //-----------------------------------------------------

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RestaurantRegisterRequestDTO restaurantRegistrationDTO)
        {
            try
            {
                RestaurantRegisterResponseDTO registerResturant = await _restaurantServices.RegistrationAsync(restaurantRegistrationDTO);
                return Ok(registerResturant);
            }
            catch (EmailAlreadyInUseException ex)
            {
                return Conflict(new { messege = ex.Message});
            }
            catch (RestaurantNameAlreadyInUseException ex)
            {
                return Conflict(new { messege = ex.Message });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RestaurantLoginRequestDTO restaurantLoginRequestDTO)
        {
            try
            {
                RestaurantLoginResponseDTO restaurantLoginResponse = await _restaurantServices.LoginAsync(restaurantLoginRequestDTO);
                return Ok(restaurantLoginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ex.Message });
            }
        }



        //-------------------------------------------------------------

        [Authorize(Roles = "Admin")]
        [HttpPatch("approve/{restaurantId}")]
        public async Task<IActionResult> ApproveRestaurant([FromRoute] int restaurantId)
        {
            try
            {
                await _restaurantServices.ApproveRestaurantAsync(restaurantId);
                return Ok(new { message = "Restaurant approved successfully." });
            }
            catch (RestaurantNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("block/{restaurantId}")]
        public async Task<IActionResult> BlockRestaurant([FromRoute] int restaurantId)
        {
            try
            {
                await _restaurantServices.BlockRestaurantAsync(restaurantId);
                return Ok(new { message = "Restaurant Blocked successfully." });
            }
            catch(RestaurantNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var restaurants = await _restaurantServices.GetAllAsync();
                return Ok(restaurants);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{restaurantId}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int restaurantId)
        {
            try
            {
                await _restaurantServices.DeleteAsync(restaurantId);
                return Ok(new {messsage = "Deleted"});

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }



        [HttpGet("{restaurantId}")]
        public async Task<IActionResult> GetRestaurant([FromRoute] int restaurantId)
        {
            try
            {
                GetRestaurantResponseDTO restaurant = await _restaurantServices.GetRestaurantAsync(restaurantId);
                return Ok(restaurant);

            }
            catch (Exception ex)
            {
               return BadRequest (new {ex.Message });
            }
        }



        //----------------------Table-------------------------

        [Authorize]
        [HttpPost("tables")]
        public async Task<IActionResult> AddTable(CreateTableReqDTO createTableReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            try
            {
                TableDTO tableDTO = new TableDTO
                {
                    RestaurantId = restaurantId,
                    Capacity = createTableReqDTO.Capacity,
                    Label = createTableReqDTO.TableLabel
                };

                CreateTableResDTO addTable = await _restaurantServices.AddTableAsync(tableDTO);
                return Ok(addTable);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize]
        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTablesByRestaurant()
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            try
            {
                IEnumerable<TableDTO> allRestaurantTables = await _restaurantServices.GetAllTablesByRestaurantIdAsync(restaurantId);
                return Ok(allRestaurantTables);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("tables/{tableId}")]
        public async Task<IActionResult> RemoveTable([FromRoute] int tableId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _restaurantServices.RemoveTableAsync(restaurantId, tableId);
                return Ok(new {message = "zai el fol etms7t"});
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (TableNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }



        //----------------------OpeningHours-------------------------

        [Authorize]
        [HttpPost("open-hours")]
        public async Task<IActionResult> AddOpeningHours(CreateRestaurantOpeningHoursReqDTO createRestaurantOpeningHoursReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                RestaurantOpeningHoursDTO restaurantOpeningHoursDTO = new RestaurantOpeningHoursDTO
                {
                    RestaurantId = restaurantId,
                    DayOfWeek = createRestaurantOpeningHoursReqDTO.DayOfWeek,
                    OpenTime = createRestaurantOpeningHoursReqDTO.OpenTime,
                    CloseTime = createRestaurantOpeningHoursReqDTO.CloseTime
                };

                await _restaurantServices.AddOpeningHours(restaurantOpeningHoursDTO);
                return Ok(new { message = "added working hours zai el fol" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [Authorize]
        [HttpPut("open-hours")]
        public async Task<IActionResult> UpdateOpeningHours(CreateRestaurantOpeningHoursReqDTO createRestaurantOpeningHoursReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                RestaurantOpeningHoursDTO restaurantOpeningHoursDTO = new RestaurantOpeningHoursDTO
                {
                    RestaurantId = restaurantId,
                    DayOfWeek = createRestaurantOpeningHoursReqDTO.DayOfWeek,
                    OpenTime = createRestaurantOpeningHoursReqDTO.OpenTime,
                    CloseTime = createRestaurantOpeningHoursReqDTO.CloseTime
                };

                await _restaurantServices.UpdateOpeningHours(restaurantOpeningHoursDTO);
                return Ok(new { message = "updated working hours zai el fol" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [Authorize]
        [HttpDelete("open-hours/{dayOfWeekEnum}")]
        public async Task<IActionResult> DeleteOpeningHours([FromRoute] DayOfWeek dayOfWeekEnum)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _restaurantServices.DeleteOpeningHours(restaurantId, dayOfWeekEnum);
                return Ok(new { message = "deleted working hours zai el fol" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }


        //-----------------RestaurantReviews-----------------------------
        [Authorize]
        [HttpPost("review")]
        public async Task<IActionResult> AddReview([FromBody]CreateRestaurantReviewReqDTO createRestaurantOpeningHoursReqDTO)
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);

            try
            {
                await _restaurantServices.AddReviewAsync(customerId , createRestaurantOpeningHoursReqDTO);
                return Ok(new { message = "Review Added" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

    }
}
