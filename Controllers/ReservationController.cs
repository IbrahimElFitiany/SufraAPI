
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

        [Authorize (Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllReservations([FromQuery] ReservationQueryDTO queryDTO)
        {
            IEnumerable<ReservationDTO> reservations = await _reservationServices.GetAllAsync(queryDTO);
            return Ok(reservations);
        }

        [Authorize (Roles = RoleNames.RestaurantManager)]
        [HttpPatch("approve/{reservationId}")]
        public async Task<IActionResult> ApproveReservation([FromRoute] int reservationId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            await _reservationServices.ApproveAsync(reservationId , restaurantId);
            return Ok(new{messasge = "approved , email sent"});
        }

        [Authorize(Roles = RoleNames.RestaurantManager)]
        [HttpPatch("reject/{reservationId}")]
        public async Task<IActionResult> RejectReservation([FromRoute] int reservationId)
        {
            int restaurantId = int.Parse(User.FindFirst("RestaurantId")?.Value);

            await _reservationServices.RejectAsync(reservationId, restaurantId);
            return Ok(new { messasge = "rejected" });
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPatch("cancel/{reservationId}")]
        public async Task<IActionResult> CancelReservation([FromRoute] int reservationId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _reservationServices.CancelAsync(reservationId, userId);
            return Ok(new { message = "canceled" });
        }

    }
}