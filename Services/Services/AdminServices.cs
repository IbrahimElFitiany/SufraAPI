using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.Infrastructure.Services;
using Sufra.Models.Emps;
using Sufra.Exceptions;
using Sufra.DTOs.SufraEmpDTOs;

namespace Sufra.Services.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly ISufraEmpRepository _sufraEmpRepository;
        private readonly ITokenService _tokenService;


        public AdminServices(ISufraEmpRepository sufraEmpRepository,ITokenService tokenService)
        {  
            _sufraEmpRepository = sufraEmpRepository;
            _tokenService = tokenService;
        }

        //------------------------------------------
        public async Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO)
        {
            SufraEmp admin = await _sufraEmpRepository.GetAdminByEmail(adminLoginRequestDTO.Email);

            if (admin == null || !(adminLoginRequestDTO.Password == admin.Password))
                throw new AuthenticationException("User not found");


            var token = _tokenService.GenerateAccessToken(
                new AdminClaimsDTO
                {
                    UserId = admin.Id,
                    Name = admin.Fname,
                    Email = admin.Email,
                    Role = admin.Role
                });

            return new AdminLoginResponseDTO
            {
                AdminID = admin.Id,
                Name = admin.Fname,
                Email = admin.Email,
                Role = admin.Role,
                Token = token
            };

        }

    }
}
