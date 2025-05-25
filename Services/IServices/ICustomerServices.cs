using SufraMVC.DTOs;

namespace SufraMVC.Services.IServices
{
    public interface ICustomerServices
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDTO);

    }
}
