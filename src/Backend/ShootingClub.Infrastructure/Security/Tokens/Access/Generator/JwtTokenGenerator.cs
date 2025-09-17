using Microsoft.IdentityModel.Tokens;
using ShootingClub.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShootingClub.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : JwtTokenHandler, IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _signingKey;

        public JwtTokenGenerator(uint expirationTimeMinutes,string signingKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signingKey = signingKey;

        }
        public string Generate(Guid identificadorUsuario, int nivelUsuario)
        {
            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Sid, identificadorUsuario.ToString()),
                new Claim("nivel_usuario", nivelUsuario.ToString())
            };

            var tokenDescripter = new SecurityTokenDescriptor
            { 
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(securityToken);

        }
    }
}
