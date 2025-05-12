
using Sufra_MVC.Models.Emps;

namespace Sufra_MVC.Repositories
{
    public interface ISufraEmpRepository
    {
        Task<SufraEmp> GetAdminByEmail(string email);
        Task<SufraEmp> GetEmpByEmail(string email);
    }
}