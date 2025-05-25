
using Models.Reservation;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Repositories
{
    public interface IReservationRepository
    {
        Task CreateAsync(Reservation reservation);
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetApprovedReservationByTableAsync(Table table);
        Task<IEnumerable<Reservation>> GetPendingReservationsByTableAsync(Table table);
        Task<IEnumerable<Reservation>> GetAllAsync();

        Task ApproveAsync(Reservation reservation);
        Task RejectAsync(Reservation reservation);
        Task CancelAsync(Reservation reservation);

        Task RescheduleReservationAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }

}