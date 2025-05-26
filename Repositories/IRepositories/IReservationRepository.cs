using Sufra.DTOs.ReservationDTOs;
using Sufra.Models.Reservations;
using Sufra.Models.Restaurants;

namespace Sufra.Repositories.IRepositories
{
    public interface IReservationRepository
    {
        Task CreateAsync(Reservation reservation);
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetApprovedReservationByTableAsync(Table table);
        Task<IEnumerable<Reservation>> GetPendingReservationsByTableAsync(Table table);
        Task<IEnumerable<Reservation>> GetAllAsync(ReservationQueryDTO queryDTO);

        Task ApproveAsync(Reservation reservation);
        Task RejectAsync(Reservation reservation);
        Task CancelAsync(Reservation reservation);

        Task RescheduleReservationAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }

}