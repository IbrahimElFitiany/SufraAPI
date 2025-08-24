using Microsoft.AspNetCore.Authorization;
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
            RestaurantRegisterResponseDTO registerResturant = await _restaurantServices.RegistrationAsync(restaurantRegistrationDTO);
            return Ok(registerResturant);
        }

        //-------------------------------------------------------------

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPatch("approve/{restaurantId}")]
        public async Task<IActionResult> ApproveRestaurant([FromRoute] int restaurantId)
        {
            await _restaurantServices.ApproveRestaurantAsync(restaurantId);
            return Ok(new { message = "Restaurant approved successfully." });
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPatch("block/{restaurantId}")]
        public async Task<IActionResult> BlockRestaurant([FromRoute] int restaurantId)
        {
            await _restaurantServices.BlockRestaurantAsync(restaurantId);
            return Ok(new { message = "Restaurant Blocked successfully." });
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> QueryRestaurants([FromQuery] RestaurantQueryDTO restaurantQueryDTO)
        {
            var restaurantlistItems = await _restaurantServices.QueryRestaurantsAsync(restaurantQueryDTO);
            return Ok(restaurantlistItems);
        }

        [AllowAnonymous]
        [HttpGet ("search")] 
        public async Task<IActionResult> SearchRestaurantAsync([FromQuery] RestaurantQueryDTO restaurantQueryDTO)
        {
            restaurantQueryDTO.IsApproved = true;
            var searchResults = await _restaurantServices.QueryRestaurantsAsync(restaurantQueryDTO);
            return Ok(searchResults);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("{restaurantId}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int restaurantId)
        {
            await _restaurantServices.DeleteAsync(restaurantId);
            return Ok(new {messsage = "Deleted"});
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpPatch("{restaurantId}")] 
        public async Task<IActionResult> UpdateRestaurant(int restaurantId, [FromBody] UpdateRestaurantReqDTO updateRestaurantReqDTO) //partial updates
        {
            await _restaurantServices.UpdateRestaurantAsync(restaurantId, updateRestaurantReqDTO);
            return Ok(new { messsage = "Restaurant Updated" });
        }

        [AllowAnonymous]
        [HttpGet("{restaurantId}")]
        public async Task<IActionResult> GetRestaurant([FromRoute] int restaurantId)
        {
            GetRestaurantResponseDTO restaurant = await _restaurantServices.GetRestaurantAsync(restaurantId);
            return Ok(restaurant);
        }

        [HttpGet("sufra-picks")]
        public async Task<IActionResult> GetSufraPicks()
        {
            IEnumerable<RestaurantListItemDTO> sufarPicks = await _restaurantServices.GetSufraPicksAsync();
            return Ok(sufarPicks);
        }


        //----------------------Table-------------------------

        [Authorize (Roles = RoleNames.RestaurantManager)]
        [HttpPost("tables")]
        public async Task<IActionResult> AddTable(CreateTableReqDTO createTableReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            TableDTO tableDTO = new TableDTO
            {
                RestaurantId = restaurantId,
                Capacity = createTableReqDTO.Capacity,
                Label = createTableReqDTO.TableLabel
            };

            CreateTableResDTO addTable = await _restaurantServices.AddTableAsync(tableDTO);
            return Ok(addTable);
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTablesByRestaurant() //will add pagination if performance or data size becomes an issue
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            IEnumerable<TableDTO> allRestaurantTables = await _restaurantServices.GetAllTablesByRestaurantIdAsync(restaurantId);
            return Ok(allRestaurantTables);
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpDelete("tables/{tableId}")]
        public async Task<IActionResult> RemoveTable([FromRoute] int tableId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            await _restaurantServices.RemoveTableAsync(restaurantId, tableId);
            return Ok(new {message = "Table Deleted"});
        }


        //----------------------OpeningHours-------------------------

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPost("opening-hours")]
        public async Task<IActionResult> AddOpeningHours(CreateRestaurantOpeningHoursReqDTO createRestaurantOpeningHoursReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

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

        [HttpGet("{restaurantId}/opening-hours")]
        public async Task<IActionResult> GetOpeningHours([FromRoute] int restaurantId)
        {
            var openingHours = await _restaurantServices.GetOpeningHours(restaurantId);
            return Ok(openingHours);
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPut("opening-hours")]
        public async Task<IActionResult> UpdateOpeningHours(CreateRestaurantOpeningHoursReqDTO createRestaurantOpeningHoursReqDTO)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

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

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpDelete("opening-hours/{dayOfWeek}")]
        public async Task<IActionResult> DeleteOpeningHours([FromRoute] DayOfWeek dayOfWeek)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            await _restaurantServices.DeleteOpeningHours(restaurantId, dayOfWeek);
            return Ok(new { message = "deleted working hours zai el fol" });
        }

        //-----------------RestaurantReviews-----------------------------

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("review/{restaurantId}")]
        public async Task<IActionResult> AddReview([FromBody]CreateRestaurantReviewReqDTO createRestaurantReviewReqDTO , [FromRoute] int restaurantId)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _restaurantServices.AddReviewAsync(customerId, restaurantId, createRestaurantReviewReqDTO);
            return Ok(new { message = "Review Added" });
        }

    }
}