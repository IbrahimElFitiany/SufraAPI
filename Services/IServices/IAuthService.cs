﻿using Sufra.Common.Types;
using Sufra.DTOs;

namespace Sufra.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResult<TLoginResDTO>> LoginAsync<TLoginResDTO>(LoginReqDTO loginReqDTO,string userAgent,string ip,string? oldToken);
        Task<RefreshResult> RefreshAsync(string oldRefreshToken, string? ip, string? userAgent);
        Task LogoutAsync(string token);
    }
}
