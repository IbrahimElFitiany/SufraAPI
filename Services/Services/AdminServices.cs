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
        private readonly JwtServices _JwtService;


        public AdminServices(ISufraEmpRepository sufraEmpRepository,JwtServices JwtService)
        {  
            _sufraEmpRepository = sufraEmpRepository;
            _JwtService = JwtService;
        }

        //------------------------------------------
        public async Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO)
        {
            SufraEmp admin = await _sufraEmpRepository.GetAdminByEmail(adminLoginRequestDTO.Email);

            if (admin == null)
                throw new UserNotFoundException("User not found");

            //sufraEmps Passwords Are not Hashed rn 
            bool isPasswordValid = adminLoginRequestDTO.Password == admin.Password;


            if (!isPasswordValid)
                throw new AuthenticationException("password mismatch");


            var token = _JwtService.GenerateToken(
                new AdminClaimsDTO
                {
                    userID = admin.Id,
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
