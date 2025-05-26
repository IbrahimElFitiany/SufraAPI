using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs;
using Sufra.DTOs.RestaurantDTOs.TableDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;

namespace Sufra.Controllers
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
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (AlreadyApprovedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                //will implement logger later
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
            catch(RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (AlreadyBlockedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                //will implement logger later
                return StatusCode(500, new { error = ex.Message });
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> QueryRestaurants([FromQuery] RestaurantQueryDTO restaurantQueryDTO)
        {
            try
            {
                var restaurantlistItems = await _restaurantServices.QueryRestaurantsAsync(restaurantQueryDTO);
                return Ok(restaurantlistItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet ("search")] 
        public async Task<IActionResult> SearchRestaurantAsync([FromQuery] RestaurantQueryDTO restaurantQueryDTO)
        {
            try
            {
                restaurantQueryDTO.IsApproved = true;
                var restaurantlistItems = await _restaurantServices.QueryRestaurantsAsync(restaurantQueryDTO);
                return Ok(restaurantlistItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
            catch (RestaurantNotFoundException ex)
            {
                return NotFound (new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{restaurantId}")] 
        public async Task<IActionResult> UpdateRestaurant(int restaurantId, [FromBody] UpdateRestaurantReqDTO updateRestaurantReqDTO) //partial updates
        {
            try
            {
                await _restaurantServices.UpdateRestaurantAsync(restaurantId, updateRestaurantReqDTO);
                return Ok(new { messsage = "Restaurant Updated" });

            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [AllowAnonymous]
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

        [HttpGet("sufra-picks")]
        public async Task<IActionResult> GetSufraPicks()
        {
            try
            {
                IEnumerable<RestaurantDTO> sufarPicks = await _restaurantServices.GetSufraPicksAsync();
                return Ok(sufarPicks);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        //----------------------Table-------------------------

        [Authorize (Roles = "RestaurantManager")]
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
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [Authorize(Roles = "RestaurantManager")]
        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTablesByRestaurant() //will add pagination if performance or data size becomes an issue
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

        [Authorize(Roles = "RestaurantManager")]
        [HttpDelete("tables/{tableId}")]
        public async Task<IActionResult> RemoveTable([FromRoute] int tableId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _restaurantServices.RemoveTableAsync(restaurantId, tableId);
                return Ok(new {message = "Table Deleted"});
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

        [Authorize(Roles = "RestaurantManager")]
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
                return Ok(new { message = "added working hours" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (OpeningHoursExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [Authorize(Roles = "RestaurantManager")]
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
                return Ok(new { message = "updated working hours" });
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

        [Authorize(Roles = "RestaurantManager")]
        [HttpDelete("open-hours/{dayOfWeek}")]
        public async Task<IActionResult> DeleteOpeningHours([FromRoute] DayOfWeek dayOfWeek)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            try
            {
                await _restaurantServices.DeleteOpeningHours(restaurantId, dayOfWeek);
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
