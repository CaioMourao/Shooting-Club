using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShootingClub.Infrastructure.Services.LoggedUsuario
{
    public class LoggedUsuario : ILoggedUsuario
    {

        private readonly ShootingClubDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;
        public LoggedUsuario(ShootingClubDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }
        public async Task<Usuario> Usuario()
        {
            var token = _tokenProvider.Value();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identificador = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var identificadorUsuario = Guid.Parse(identificador);

            return await _dbContext
                .Usuarios
                .AsNoTracking()
                .FirstAsync(usuario => usuario.Ativo && usuario.IdentificadorUsuario == identificadorUsuario);
        }
    }
}
