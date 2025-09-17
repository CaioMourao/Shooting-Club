using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.Delete
{
    public class DeleteUsuarioUseCase : IDeleteUsuarioUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioReadOnlyRepository _repositoryRead;
        private readonly IUsuarioWriteOnlyRepository _repositoryWrite;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUsuarioUseCase(ILoggedUsuario loggedUsuario, IUsuarioReadOnlyRepository repositoryRead, IUsuarioWriteOnlyRepository repositoryWrite, IUnitOfWork unitOfWork)
        {
            _loggedUsuario = loggedUsuario;
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _unitOfWork = unitOfWork;

        }
        public async Task Execute(int usuarioId)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            var canDelete = await _repositoryRead.CanDelete(loggedUsuario, usuarioId);

            if (!canDelete)
            {
                throw new NotFoundException(ResourceMessagesException.USUARIO_NAO_ENCONTRADO);
            }

            await _repositoryWrite.Delete(usuarioId);

            await _unitOfWork.Commit();
        }
    }
}
