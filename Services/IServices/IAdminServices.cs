using SufraMVC.DTOs;

namespace SufraMVC.Services.IServices
{
    public interface IAdminServices
    {
        Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO);
    }
}
