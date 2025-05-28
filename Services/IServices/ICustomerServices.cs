using Sufra.DTOs.CustomerDTOs;

namespace Sufra.Services.IServices
{
    public interface ICustomerServices
    {
        Task<CustomerLoginResDTO> LoginAsync(CustomerLoginReqDTO loginDTO);
        Task<RegisterResponseDTO> RegisterAsync(CustomerRegisterDTO registerDTO);

    }
}
