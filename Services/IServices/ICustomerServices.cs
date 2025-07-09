using Sufra.Common.Types;
using Sufra.DTOs.CustomerDTOs;

namespace Sufra.Services.IServices
{
    public interface ICustomerServices
    {
        Task<RegisterResponseDTO> RegisterAsync(CustomerRegisterDTO registerDTO);
    }
}
