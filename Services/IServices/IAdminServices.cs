using Sufra.DTOs;

namespace Sufra.Services.IServices
{
    public interface IAdminServices
    {
        Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO);
    }
}
