using Microsoft.EntityFrameworkCore;
using SufraMVC.Data;
using SufraMVC.Models;
using SufraMVC.Repositories.IRepositories;

namespace SufraMVC.Repositories.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly Sufra_DbContext _context;

        public DistrictRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------------

        public Task CreateAsync(District cuisine)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<District>> GetAllAsync()
        {
            return await _context.Districts.ToListAsync();
        }

        public Task<District> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(District cuisine)
        {
            throw new NotImplementedException();
        }

        //------------------------



    }
}
