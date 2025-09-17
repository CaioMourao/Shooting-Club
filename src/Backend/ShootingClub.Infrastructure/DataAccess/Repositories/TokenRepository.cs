using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Token;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public TokenRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;

        public async Task<RefreshToken?> Get(string refreshToken)
        {
            return await _dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.usuario)
                .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = _dbContext.RefreshTokens.Where(token => token.UsuarioId == refreshToken.UsuarioId);
            _dbContext.RefreshTokens.RemoveRange(tokens);
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
        }
    }
}
