using Microsoft.AspNetCore.Mvc;
using Sufra.DTOs;
using Sufra.DTOs.CustomerDTOs;
using Sufra.DTOs.SufraEmpDTOs;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Sufra.Common.Types;
using Sufra.Exceptions.Auth;
using Sufra.Exceptions;
using Sufra.Common.Constants;
using System.Security.Claims;

namespace Sufra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        //--------

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO loginDto)
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            string? oldToken = Request.Cookies["refreshToken"];

            _logger.LogInformation("User-Agent: {UserAgent}", userAgent);
            _logger.LogInformation("IP Address: {IP}", ip);

            switch (loginDto.UserType)
            {
                case RoleNames.Customer:
                    var customerLoginResult = await _authService.LoginAsync<CustomerLoginResDTO>(loginDto, userAgent, ip, oldToken);
                    return HandleLoginResponse(customerLoginResult);

                case RoleNames.Admin:
                    var adminLoginResult = await _authService.LoginAsync<AdminLoginResponseDTO>(loginDto, userAgent, ip, oldToken);
                    return HandleLoginResponse(adminLoginResult);

                case RoleNames.RestaurantManager:
                    var managerLoginResult = await _authService.LoginAsync<RestaurantLoginResponseDTO>(loginDto, userAgent, ip, oldToken);
                    return HandleLoginResponse(managerLoginResult);

                default:
                    return BadRequest(new { message = "Invalid user type."});
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role)) return Unauthorized();
            if (!int.TryParse(userId, out int userID)) return Unauthorized();

            switch (role)
            {
                case RoleNames.Customer:
                    var customerLoginResult = await _authService.GetMeAsync<CustomerLoginResDTO>(userID, role);
                    return Ok(customerLoginResult.MeRes);

                case RoleNames.Admin:
                    var adminLoginResult = await _authService.GetMeAsync<AdminLoginResponseDTO>(userID, role);
                    return Ok(adminLoginResult.MeRes);

                case RoleNames.RestaurantManager:
                    var managerLoginResult = await _authService.GetMeAsync<RestaurantLoginResponseDTO>(userID, role);
                    return Ok(managerLoginResult.MeRes);

                default:
                    return BadRequest(new { message = "Invalid user type." });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) throw new CookieNotFoundException();

            string? ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-Agent"].ToString();

            var refreshResult = await _authService.RefreshAsync(refreshToken, ip, userAgent);

            Response.Cookies.Append("refreshToken", refreshResult.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.None,
                Expires = refreshResult.ExpirationTime
            });

            Response.Cookies.Append("accessToken", refreshResult.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.None,
            });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken)) return BadRequest(new { message = "No refresh token found." });

            await _authService.LogoutAsync(refreshToken);

            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("accessToken");

            return Ok(new { message = "Logged out successfully." });
        }

        private IActionResult HandleLoginResponse<T>(LoginResult<T> result)
        {
            Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.None,
                Expires = result.ExpirationDate
            });

            Response.Cookies.Append("accessToken", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.None
            });

            return Ok(result.LoginResDTO);
        }
    }
}
