using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Arma.Register
{
    public interface IRegisterArmaUseCase
    {
        public Task<ResponseRegisteredArmaJson> Execute(RequestArmaBaseJson request);
    }
}
