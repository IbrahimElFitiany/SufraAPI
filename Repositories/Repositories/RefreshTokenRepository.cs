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

        public Task<RefreshToken?> GetByTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}