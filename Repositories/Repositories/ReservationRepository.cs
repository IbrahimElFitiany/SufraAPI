using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra.Data;
using Sufra.DTOs.ReservationDTOs;
using Sufra.Models.Reservations;
using Sufra.Repositories.IRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Table = Sufra.Models.Restaurants.Table;

namespace Sufra.Repositories.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly Sufra_DbContext _context;

        public ReservationRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------

        public async Task CreateAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }


        public async Task<Reservation> GetByIdAsync(int id)
        {
            Reservation reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            return reservation;
        }
        public async Task<IEnumerable<Reservation>> GetApprovedReservationByTableAsync( Table table)
        {
            IEnumerable<Reservation> reservations = await _context.Reservations.Where(r => r.TableId == table.Id && r.Status == ReservationStatus.Approved).ToListAsync();
            return reservations;
        }
        public async Task<IEnumerable<Reservation>> GetPendingReservationsByTableAsync(Table table)
        {
            IEnumerable<Reservation> reservations = await _context.Reservations.Where(r => r.TableId == table.Id && r.Status == ReservationStatus.Pending).ToListAsync();
            return reservations;
        }
        public async Task<IEnumerable<Reservation>> GetAllAsync(ReservationQueryDTO queryDTO)
        {
            IQueryable<Reservation> reservations = _context.Reservations;

            int skip = (queryDTO.Page - 1) * queryDTO.PageSize;

            reservations = reservations.Skip(skip).Take(queryDTO.PageSize);

            return await reservations.ToListAsync();
        }

        public async Task ApproveAsync(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Approved;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task RejectAsync(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Rejected;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task CancelAsync(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Canceled;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task RescheduleReservationAsync(Reservation reservation)
        {
            throw new NotImplementedException();
        }



        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

    }
}
