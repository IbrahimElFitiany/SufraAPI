using SufraMVC.Models.Emps;

namespace SufraMVC.Repositories.IRepositories
{
    public interface ISufraEmpRepository
    {
        Task<SufraEmp> GetAdminByEmail(string email);
        Task<SufraEmp> GetEmpByEmail(string email);
    }
}