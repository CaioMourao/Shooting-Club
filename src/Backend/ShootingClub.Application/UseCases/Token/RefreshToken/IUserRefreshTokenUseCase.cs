using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Security.Tokens;

namespace ShootingClub.Application.UseCases.Token.RefreshToken
{
    public interface IUserRefreshTokenUseCase
    {
        public Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
    }
}
