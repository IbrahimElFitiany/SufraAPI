using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.Models.Emps;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class SufraEmpRepository : ISufraEmpRepository
    {
        private readonly Sufra_DbContext _context;

        public SufraEmpRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------------------------------
        public async Task<SufraEmp> GetAdminByEmail(string email)
        {
            return await _context.Sufra_Emps.Where(e => e.Email == email && e.Role == "Admin").FirstOrDefaultAsync();
        }
        public async Task<SufraEmp> GetAdminById(int id)
        {
            return await _context.Sufra_Emps.Where(e => e.Id == id && e.Role == "Admin").FirstOrDefaultAsync();
        }

        public async Task<SufraEmp> GetEmpByEmail(string email)
        {
            return await _context.Sufra_Emps.Where(e => e.Email == email && e.Role == "Emp" || e.Role == "Support").FirstOrDefaultAsync();
        }
    }
}
