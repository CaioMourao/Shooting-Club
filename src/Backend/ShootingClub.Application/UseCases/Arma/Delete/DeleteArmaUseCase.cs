
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Arma.Delete
{
    public class DeleteArmaUseCase : IDeleteArmaUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IArmaReadOnlyRepository _repositoryRead;
        private readonly IArmaWriteOnlyRepository _repositoryWrite;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteArmaUseCase(ILoggedUsuario loggedUsuario, IArmaReadOnlyRepository repositoryRead, IArmaWriteOnlyRepository repositoryWrite, IUnitOfWork unitOfWork)
        {
            _loggedUsuario = loggedUsuario;
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _unitOfWork = unitOfWork;

        }
        public async Task Execute(int armaId)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            var canDelete = await _repositoryRead.CanDelete(loggedUsuario, armaId);

            if (!canDelete)
            {
                throw new NotFoundException(ResourceMessagesException.ARMA_NAO_ENCONTRADA);
            }

            await _repositoryWrite.Delete(armaId);

            await _unitOfWork.Commit();
        }
    }
}
