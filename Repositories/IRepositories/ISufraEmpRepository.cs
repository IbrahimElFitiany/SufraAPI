using Sufra.Models.Emps;

namespace Sufra.Repositories.IRepositories
{
    public interface ISufraEmpRepository
    {
        Task<SufraEmp> GetAdminByEmail(string email);
        Task<SufraEmp> GetEmpByEmail(string email);
    }
}