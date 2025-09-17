using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.Update
{
    public class UpdateUsuarioUseCase : IUpdateUsuarioUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioUpdateOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUsuarioUseCase(ILoggedUsuario loggedUsuario, IUsuarioUpdateOnlyRepository repository, IUsuarioReadOnlyRepository usuarioReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _loggedUsuario = loggedUsuario;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateUsuarioJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            Validate(request, loggedUsuario.Email, loggedUsuario.CPF, loggedUsuario.DataFiliacao);

            var usuario = await _repository.GetById(loggedUsuario.Id);

            usuario.AtualizadoEm = DateTime.UtcNow;
            usuario.Nome = request.Nome;
            usuario.DataNascimento = request.DataNascimento;
            usuario.EnderecoPais = request.EnderecoPais;
            usuario.EnderecoEstado = request.EnderecoEstado;
            usuario.EnderecoCidade = request.EnderecoCidade;
            usuario.EnderecoBairro= request.EnderecoBairro;
            usuario.EnderecoRua = request.EnderecoRua;
            usuario.EnderecoNumero = request.EnderecoNumero;

            _repository.Update(usuario);

            await _unitOfWork.Commit();
        }

        private static void Validate(RequestUpdateUsuarioJson request, string CurrentEmail, string CurrentCPF, DateOnly CurrentDataFiliacao)
        {
            var validator = new UpdateUsuarioValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
