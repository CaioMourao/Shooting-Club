using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Usuario.Update
{
    public interface IUpdateUsuarioUseCase
    {
        public Task Execute(RequestUpdateUsuarioJson request);
    }
}
