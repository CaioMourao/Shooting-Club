using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Usuario.Profile
{
    public interface IGetUsuarioProfileUseCase
    {
        public Task<ResponseUsuarioProfileJson> Execute();
    }
}
