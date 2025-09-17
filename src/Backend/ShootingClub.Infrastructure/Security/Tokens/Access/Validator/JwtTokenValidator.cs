using Microsoft.IdentityModel.Tokens;
using ShootingClub.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShootingClub.Infrastructure.Security.Tokens.Access.Validator
{
    public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
    {
        private readonly string _signingkey;
        public JwtTokenValidator(string signingkey) => _signingkey = signingkey;

        public (Guid IdentificadorUsuario, int NivelUsuario) ValidateAndGetIdentificadorUsuarioAndNivel(string token)
        {
            var validationParameter = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_signingkey),
                ClockSkew = new TimeSpan(0)
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

            var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var nivelClaim = principal.Claims.FirstOrDefault(c => c.Type == "nivel_usuario")?.Value;

            int nivelUsuario = 0;
            if (!string.IsNullOrEmpty(nivelClaim))
                int.TryParse(nivelClaim, out nivelUsuario);

            return (Guid.Parse(userIdentifier), nivelUsuario);
        }
    }
}
