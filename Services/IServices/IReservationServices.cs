
using Sufra.DTOs.ReservationDTOs;
using Sufra.Models.Reservations;

namespace Sufra.Services.IServices
{
    public interface IReservationServices
    {
        Task<CreateReservationResDTO> CreateAsync(ReservationDTO reservationDTO);

        Task ApproveAsync(int reservationId, int restaurantId);
        Task RejectAsync(int reservationId, int restaurantId);
        Task CancelAsync(int reservationId, int customerId);
        Task<IEnumerable<ReservationDTO>> GetAllAsync(ReservationQueryDTO queryDTO);

        Task RescheduleAsync(Reservation reservation);
    }
}
