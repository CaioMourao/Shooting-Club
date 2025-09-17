using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Usuario.UpdateByAdmin
{
    public interface IUsuarioUpdateByAdminUseCase
    {
        public Task Execute(int id, RequestUsuarioJson request);
    }
}
