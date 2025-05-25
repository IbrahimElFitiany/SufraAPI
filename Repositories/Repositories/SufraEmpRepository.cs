using Microsoft.EntityFrameworkCore;
using SufraMVC.Data;
using SufraMVC.Models.Emps;
using SufraMVC.Repositories.IRepositories;


namespace SufraMVC.Repositories.Repositories
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
            return await _context.Sufra_Emps.Where(e => e.Email == email && e.Role == "Admin").FirstAsync();
        }

        public async Task<SufraEmp> GetEmpByEmail(string email)
        {
            return await _context.Sufra_Emps.Where(e => e.Email == email && e.Role == "Emp" || e.Role == "Support").FirstAsync();
        }
    }
}
