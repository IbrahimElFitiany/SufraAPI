using Sufra.Common.Types;
using Sufra.DTOs.CustomerDTOs;

namespace Sufra.Services.IServices
{
    public interface ICustomerServices
    {
        Task<LoginResult<CustomerLoginResDTO>> LoginAsync(CustomerLoginReqDTO loginDTO);
        Task<RegisterResponseDTO> RegisterAsync(CustomerRegisterDTO registerDTO);

    }
}
