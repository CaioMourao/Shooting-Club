using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Usuario.GetById
{
    public interface IGetUsuarioByIdUseCase
    {
        public Task<ResponseUsuarioJson> Execute(int UsuarioId);
    }
}
