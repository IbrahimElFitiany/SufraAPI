using Sufra.Common.Constants;
using Sufra.Common.Types;
using Sufra.DTOs;
using Sufra.DTOs.CustomerDTOs;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.DTOs.SufraEmpDTOs;
using Sufra.Exceptions;
using Sufra.Exceptions.Auth;
using Sufra.Infrastructure.Services;
using Sufra.Models;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;

namespace Sufra.Services.Services
{
    public class AuthService:IAuthService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ISufraEmpRepository _adminRepo;
        private readonly IRestaurantManagerRepository _restaurantManagerRepo;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public AuthService(ICustomerRepository customerRepo,ISufraEmpRepository adminRepo, IRestaurantManagerRepository restaurantManagerRepo,ITokenService tokenService,IRefreshTokenRepository refreshTokenRepo)
        {
            _customerRepo = customerRepo;
            _adminRepo = adminRepo;
            _restaurantManagerRepo = restaurantManagerRepo;
            _tokenService = tokenService;
            _refreshTokenRepo = refreshTokenRepo;
        }

        //----------

        public async Task<LoginResult<TLoginResDTO>> LoginAsync<TLoginResDTO>(LoginReqDTO loginReqDTO , string userAgent , string ip, string? oldToken)
        {
            int userId;
            string accessToken;
            object loginResDTO;

            switch (loginReqDTO.UserType)
            {
                case RoleNames.Customer:
                    var customer = await _customerRepo.GetCustomerByEmailAsync(loginReqDTO.Email);
                    if (customer == null || !BCrypt.Net.BCrypt.Verify(loginReqDTO.Password, customer.Password))
                        throw new AuthenticationException("Invalid email or password.");

                    userId = customer.Id;

                    accessToken = _tokenService.GenerateAccessToken(new CustomerClaimsDTO
                    {
                        UserId = customer.Id,
                        Name = customer.Fname,
                        Email = customer.Email,
                        Role = RoleNames.Customer
                    });

                    loginResDTO = new CustomerLoginResDTO
                    {
                        Fname = customer.Fname,
                        Lname = customer.Lname,
                        Email = customer.Email,
                        AccessToken = accessToken
                    };
                    break;

                case RoleNames.Admin:
                    var admin = await _adminRepo.GetAdminByEmail(loginReqDTO.Email);
                    if (admin == null || loginReqDTO.Password != admin.Password)
                        throw new AuthenticationException("Invalid email or password.");

                    userId = admin.Id;

                    accessToken = _tokenService.GenerateAccessToken(new AdminClaimsDTO
                    {
                        UserId = admin.Id,
                        Name = admin.Fname,
                        Email = admin.Email,
                        Role = RoleNames.Admin
                    });

                    loginResDTO = new AdminLoginResponseDTO
                    {
                        AdminID = admin.Id,
                        Name = admin.Fname,
                        Email = admin.Email,
                        Role = RoleNames.Admin,
                        AccessToken = accessToken
                    };
                    break;

                case RoleNames.RestaurantManager:
                    var manager = await _restaurantManagerRepo.GetManagerByEmailAsync(loginReqDTO.Email);
                    if (manager == null || !BCrypt.Net.BCrypt.Verify(loginReqDTO.Password, manager.Password))
                        throw new AuthenticationException("Invalid email or password.");

                    userId = manager.Id;

                    accessToken = _tokenService.GenerateAccessToken(new RestaurantClaimsDTO
                    {
                        UserId = manager.Id,
                        Name = manager.Fname,
                        Email = manager.Email,
                        RestaurantId = manager.Restaurant.Id,
                        RestaurantName = manager.Restaurant.Name,
                        IsApproved = manager.Restaurant.IsApproved,
                        Role = RoleNames.RestaurantManager
                    });

                    loginResDTO = new RestaurantLoginResponseDTO
                    {
                        ManagerID = manager.Id,
                        ManagerName = manager.Fname,
                        Email = manager.Email,
                        RestaurantId = manager.Restaurant.Id,
                        RestaurantName = manager.Restaurant.Name,
                        IsApproved = manager.Restaurant.IsApproved,
                        AccessToken = accessToken
                    };
                    break;

                default:
                    throw new AuthenticationException("Unsupported user type.");
            }


            var refreshToken = _tokenService.GenerateRefreshToken(userId, loginReqDTO.UserType, ip, userAgent);
            refreshToken = await _refreshTokenRepo.AddAsync(refreshToken);

            if (!string.IsNullOrEmpty(oldToken))
            {
                var oldRefreshToken = await _refreshTokenRepo.GetByTokenAsync(oldToken);
                if (oldRefreshToken != null && !oldRefreshToken.IsRevoked)
                {
                    oldRefreshToken.IsRevoked = true;
                    oldRefreshToken.RevokedAt = DateTime.UtcNow;
                    oldRefreshToken.ReplacedByToken = refreshToken.Token;
                    await _refreshTokenRepo.UpdateAsync(oldRefreshToken);
                }
            }

            return new LoginResult<TLoginResDTO>
            {
                LoginResDTO = (TLoginResDTO)loginResDTO,
                RefreshToken = refreshToken.Token,
                ExpirationDate = refreshToken.ExpiresAt
            };
        }

        public async Task<RefreshResult> RefreshAsync(string oldRefreshToken,string? ip , string? userAgent)
        {
            RefreshToken? refreshToken = await _refreshTokenRepo.GetByTokenAsync(oldRefreshToken);
            if (refreshToken == null) throw new RefreshTokenNotFoundException();
            if (refreshToken.IsRevoked) throw new RevokedTokenException();
            if (refreshToken.ExpiresAt < DateTime.UtcNow) throw new ExpiredTokenException();

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;


            var newRefreshToken = _tokenService.GenerateRefreshToken(
                refreshToken.UserId,
                refreshToken.UserType,
                ip,
                userAgent
            );

            newRefreshToken = await _refreshTokenRepo.AddAsync(newRefreshToken);


            refreshToken.ReplacedByToken = newRefreshToken.Token;
            await _refreshTokenRepo.UpdateAsync(refreshToken);

            string accessToken;

            switch (refreshToken.UserType)
            {
                case RoleNames.Customer:
                    var customer = await _customerRepo.GetByIdAsync(refreshToken.UserId);
                    accessToken = _tokenService.GenerateAccessToken(new CustomerClaimsDTO
                    {
                        UserId = customer.Id,
                        Name = customer.Fname,
                        Email = customer.Email,
                        Role = RoleNames.Customer
                    });
                    break;

                case RoleNames.RestaurantManager:
                    var manager = await _restaurantManagerRepo.GetManagerByIdAsync(refreshToken.UserId);
                    accessToken = _tokenService.GenerateAccessToken(new RestaurantClaimsDTO
                    {
                        UserId = manager.Id,
                        Email = manager.Email,
                        Name = manager.Fname,
                        RestaurantId = manager.Restaurant.Id,
                        RestaurantName = manager.Restaurant.Name,
                        IsApproved = manager.Restaurant.IsApproved,
                        Role = RoleNames.RestaurantManager
                    });
                    break;

                case RoleNames.Admin:
                    var admin = await _adminRepo.GetAdminById(refreshToken.UserId);
                    accessToken = _tokenService.GenerateAccessToken(new AdminClaimsDTO
                    {
                        UserId = admin.Id,
                        Name = admin.Fname,
                        Email = admin.Email,
                        Role = RoleNames.Admin
                    });
                    break;
                default:
                    throw new AuthenticationException("Unsupported user type.");
            }

            return new RefreshResult
            {
                RefreshToken = newRefreshToken.Token,
                AccessToken = accessToken,
                ExpirationTime = newRefreshToken.ExpiresAt
            };
        }

        public async Task LogoutAsync(string token)
        {
            var refreshToken = await _refreshTokenRepo.GetByTokenAsync(token);
            if (refreshToken == null)
                throw new RefreshTokenNotFoundException() ;

            await _refreshTokenRepo.RevokeAsync(refreshToken);
        }

    }
}