using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Token;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Domain.ValueObjects;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Token.RefreshToken
{
    internal class UseRefreshTokenUseCase : IUserRefreshTokenUseCase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public UseRefreshTokenUseCase(ITokenRepository tokenRepository, IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }
        public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.Get(request.RefreshToken);

            if (refreshToken is null)
                throw new RefreshTokenNotFoundException();

            var refreshTokenValidUntil = refreshToken.CriadoEm.AddDays(ShootingClubRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
            if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
                throw new RefreshTokenNotFoundException();

            var newRefreshToken = new Domain.Entities.RefreshToken { 
                Value = _refreshTokenGenerator.Generate(),
                UsuarioId = refreshToken.UsuarioId
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

            await _unitOfWork.Commit();

            return new ResponseTokensJson 
            { 
                AccessToken = _accessTokenGenerator.Generate(refreshToken.usuario.IdentificadorUsuario, (int)refreshToken.usuario.Nivel),
                RefreshToken = newRefreshToken.Value
            };

        }
    }
}
