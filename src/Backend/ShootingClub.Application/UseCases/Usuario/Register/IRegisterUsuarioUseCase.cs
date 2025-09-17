using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Usuario.Register
{
    public interface IRegisterUsuarioUseCase
    {
        public Task<ResponseRegisteredUsuarioJson> Execute(RequestUsuarioJson request);
    }
}
