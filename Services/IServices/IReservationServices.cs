using DTOs.ReservationDTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Reservation;
using Sufra_MVC.DTOs;

namespace Sufra_MVC.Services.IServices
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
