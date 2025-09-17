using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Usuario.Filter
{
    public interface IFilterUsuarioUseCase
    {
        public Task<ResponseUsuariosJson> Execute(RequestFilterUsuarioJson request);
    }
}
