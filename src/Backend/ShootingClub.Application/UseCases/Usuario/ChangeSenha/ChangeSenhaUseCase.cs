using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Cryptography;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;
using System.Runtime.InteropServices;

namespace ShootingClub.Application.UseCases.Usuario.ChangeSenha
{
    public class ChangeSenhaUseCase : IChangeSenhaUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioUpdateOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISenhaEncripter _senhaEncripter;
        public ChangeSenhaUseCase(ILoggedUsuario loggedUsuario, IUsuarioUpdateOnlyRepository repository, IUnitOfWork unitOfWork, ISenhaEncripter senhaEncripter)
        {
            _loggedUsuario = loggedUsuario;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _senhaEncripter = senhaEncripter;
        }
        public async Task Execute(RequestChangeSenhaJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            Validate(request, loggedUsuario);

            var usuario = await _repository.GetById(loggedUsuario.Id);

            usuario.Senha = _senhaEncripter.Encrypt(request.NovaSenha);

            _repository.Update(usuario);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangeSenhaJson request, Domain.Entities.Usuario loggedUsuario)
        {
            var result = new ChangeSenhaValidator().Validate(request);

            if (!_senhaEncripter.IsValid(request.Senha, loggedUsuario.Senha))
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.SENHA_DIFERENTE_ATUAL));

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
