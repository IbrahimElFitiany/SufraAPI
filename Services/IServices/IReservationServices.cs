
using SufraMVC.DTOs.ReservationDTOs;
using SufraMVC.Models.Reservations;

namespace SufraMVC.Services.IServices
{
    public interface IReservationServices
    {
        Task<CreateReservationResDTO> CreateAsync(ReservationDTO reservationDTO);

        Task ApproveAsync(int reservationId, int restaurantId);
        Task RejectAsync(int reservationId, int restaurantId);
        Task CancelAsync(int reservationId, int customerId);
        Task<IEnumerable<ReservationDTO>> GetAllAsync();

        Task RescheduleAsync(Reservation reservation);
    }
}
