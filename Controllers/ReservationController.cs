
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sufra.Common.Constants;
using Sufra.DTOs.ReservationDTOs;
using Sufra.Exceptions;
using Sufra.Services.IServices;
using System.Security.Claims;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationServices _reservationServices;

        public ReservationController(IReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        //-----------------------------------

        [Authorize (Roles = RoleNames.Customer)]
        [HttpPost("{restaurantId}")]
        public async Task<IActionResult> CreateReservation([FromRoute] int restaurantId,[FromBody] CreateReservationReqDTO createReservationReqDTO)
        {
            int customerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                ReservationDTO reservationDTO = new ReservationDTO
                {
                    CustomerId = customerId,
                    RestaurantId = restaurantId,
                    ReservationDateTime = createReservationReqDTO.StartTime,
                    PartySize = createReservationReqDTO.PartySize,
                };

                CreateReservationResDTO createReservation = await _reservationServices.CreateAsync(reservationDTO);

                return Ok(createReservation);
            }
            catch (NoAvailableTablesException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (OutOfOpeningHoursException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (RestaurantNotApprovedException ex)
            {
                return Forbid();
            }
            catch (RestaurantNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }


        [Authorize (Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllReservations([FromQuery] ReservationQueryDTO queryDTO)
        {
            try
            {
                IEnumerable<ReservationDTO> reservations = await _reservationServices.GetAllAsync(queryDTO);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [Authorize (Roles = RoleNames.RestaurantManager)]
        [HttpPatch("approve/{reservationId}")]
        public async Task<IActionResult> ApproveReservation([FromRoute] int reservationId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            try
            {
                await _reservationServices.ApproveAsync(reservationId , restaurantId);
                return Ok(new{messasge = "approved , email sent"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPatch("reject/{reservationId}")]
        public async Task<IActionResult> RejectReservation([FromRoute] int reservationId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);
            try
            {
                await _reservationServices.RejectAsync(reservationId, restaurantId);
                return Ok(new { messasge = "rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPatch("cancel/{reservationId}")]
        public async Task<IActionResult> CancelReservation([FromRoute] int reservationId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                await _reservationServices.CancelAsync(reservationId, userId);
                return Ok(new { message = "canceled" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }
            catch (ReservationNotFoundException ex)
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

    }
}
