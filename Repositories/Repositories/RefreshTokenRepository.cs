using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.Models;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly Sufra_DbContext _context;

        public RefreshTokenRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //-------------

        public async Task<RefreshToken> AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}