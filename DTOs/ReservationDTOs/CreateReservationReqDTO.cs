using Sufra.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.ReservationDTOs
{
    public class CreateReservationReqDTO
    {
        [Required]
        [DateNotInPast]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "PartySize must be between 1 and 20.")]
        public int PartySize { get; set; }
    }
}