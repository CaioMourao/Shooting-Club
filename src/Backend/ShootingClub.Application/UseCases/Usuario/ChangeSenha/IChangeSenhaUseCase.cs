using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Usuario.ChangeSenha
{
    public interface IChangeSenhaUseCase
    {
        public Task Execute(RequestChangeSenhaJson request);
    }
}
