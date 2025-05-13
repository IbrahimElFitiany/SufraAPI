using DTOs.ReservationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Models.Reservation;
using Sufra_MVC.DTOs;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Services.IServices;
using Sufra_MVC.Services.Services;

namespace sufra.Sufra.Emps.Presentation.Controllers
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

        [Authorize]
        [HttpPost("{restaurantId}")]
        public async Task<IActionResult> CreateReservation([FromRoute] int restaurantId,[FromBody] CreateReservationReqDTO createReservationReqDTO)
        {
            int customerId = int.Parse(User.FindFirst("UserID")?.Value);
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
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPatch("cancel/{reservationId}")]
        public async Task<IActionResult> CancelReservation([FromRoute] int reservationId)
        {
            int userId = int.Parse(User.FindFirst("UserID")?.Value);
            try
            {
                await _reservationServices.CancelAsync(reservationId, userId);
                return Ok(new { messasge = "canceled" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
