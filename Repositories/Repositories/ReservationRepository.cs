using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sufra.Data;
using Sufra.DTOs.ReservationDTOs;
using Sufra.Models.Reservations;
using Sufra.Models.Restaurants;
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
        public async Task<IEnumerable<Reservation>> GetApprovedReservationsByTableAsync(Table table)
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
            IQueryable<Reservation> reservations = _context.Reservations //over-fetching will fix it later
                .Include(r => r.Customer)
                .Include(r => r.Restaurant)
                .Include(r => r.Table)
                .OrderByDescending(r => r.Id);

            if (queryDTO.Status.HasValue) reservations = reservations.Where(r => r.Status == queryDTO.Status.Value);


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


        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

    }
}
