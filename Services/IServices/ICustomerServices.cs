using Sufra.DTOs.CustomerDTOs;

namespace Sufra.Services.IServices
{
    public interface ICustomerServices
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDTO);

    }
}
