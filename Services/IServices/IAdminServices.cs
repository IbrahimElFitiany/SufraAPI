using Sufra_MVC.DTOs;

namespace Sufra_MVC.Services.IServices
{
    public interface IAdminServices
    {
        Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO);
    }
}
