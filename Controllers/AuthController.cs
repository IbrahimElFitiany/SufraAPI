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
            try
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
            catch (AuthenticationException ex)
            {
                _logger.LogWarning("Authentication failed: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
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

                return Ok(new { accessToken = refreshResult.AccessToken });
            }
            catch (ExpiredTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (RefreshTokenNotFoundException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (RevokedTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try{
                string? refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken)) return BadRequest(new { message = "No refresh token found." });

                await _authService.LogoutAsync(refreshToken);

                Response.Cookies.Delete("refreshToken");

                return Ok(new { message = "Logged out successfully." });
            }
            catch (RefreshTokenNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
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

            return Ok(result.LoginResDTO);
        }
    }
}
