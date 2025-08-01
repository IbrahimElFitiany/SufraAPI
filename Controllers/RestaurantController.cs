﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sufra.Common.Constants;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs;
using Sufra.DTOs.RestaurantDTOs.TableDTOs;
using Sufra.Exceptions;
using Sufra.Models.Restaurants;
using Sufra.Services.IServices;
using System.Security.Claims;

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

        [AllowAnonymous]
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


        //-------------------------------------------------------------

        [Authorize(Roles = RoleNames.Admin)]
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

        [Authorize(Roles = RoleNames.Admin)]
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


        [Authorize(Roles = RoleNames.Admin)]
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
                var searchResults = await _restaurantServices.QueryRestaurantsAsync(restaurantQueryDTO);
                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = RoleNames.Admin)]
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


        [Authorize(Roles = RoleNames.Admin)]
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
                IEnumerable<RestaurantListItemDTO> sufarPicks = await _restaurantServices.GetSufraPicksAsync();
                return Ok(sufarPicks);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        //----------------------Table-------------------------

        [Authorize (Roles = RoleNames.RestaurantManager)]
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPost("opening-hours")]
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

        [HttpGet("{restaurantId}/opening-hours")]
        public async Task<IActionResult> GetOpeningHours([FromRoute] int restaurantId)
        {
            try
            {
                var openingHours = await _restaurantServices.GetOpeningHours(restaurantId);
                return Ok(openingHours);
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPut("opening-hours")]
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

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpDelete("opening-hours/{dayOfWeek}")]
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

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("review/{restaurantId}")]
        public async Task<IActionResult> AddReview([FromBody]CreateRestaurantReviewReqDTO createRestaurantReviewReqDTO , [FromRoute] int restaurantId)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                await _restaurantServices.AddReviewAsync(customerId, restaurantId, createRestaurantReviewReqDTO);
                return Ok(new { message = "Review Added" });
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (CustomerAlreadyReviewed ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

    }
}
