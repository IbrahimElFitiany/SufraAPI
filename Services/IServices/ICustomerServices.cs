using Sufra_MVC.DTOs;

namespace Sufra_MVC.Services.IServices
{
    public interface ICustomerServices
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDTO);

    }
}
