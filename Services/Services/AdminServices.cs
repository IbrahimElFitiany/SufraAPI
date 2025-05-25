using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.Infrastructure.Services;
using Sufra.DTOs;

namespace Sufra.Services.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly ISufraEmpRepository _sufraEmpRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly JwtServices _JwtService;


        public AdminServices(ISufraEmpRepository sufraEmpRepository, IRestaurantRepository restaurantRepository, JwtServices JwtService)
        {  
            _sufraEmpRepository = sufraEmpRepository;
            _restaurantRepository = restaurantRepository;
            _JwtService = JwtService;
        }

        //------------------------------------------
        public async Task<AdminLoginResponseDTO> Login(AdminLoginRequestDTO adminLoginRequestDTO)
        {
            var admin = await _sufraEmpRepository.GetAdminByEmail(adminLoginRequestDTO.Email);

            if (admin == null)
                throw new Exception("no admin with this email");


            bool isPasswordValid = adminLoginRequestDTO.Password == admin.Password;


            if (!isPasswordValid)
                throw new Exception("password mismatch");



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
