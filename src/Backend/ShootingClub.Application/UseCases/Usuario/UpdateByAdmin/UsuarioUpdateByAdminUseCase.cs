using AutoMapper;
using ShootingClub.Application.UseCases.Usuario.Update;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Cryptography;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.UpdateByAdmin
{
    public class UsuarioUpdateByAdminUseCase : IUsuarioUpdateByAdminUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioUpdateOnlyRepository _repository;
        private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISenhaEncripter _senhaEncripter;

        public UsuarioUpdateByAdminUseCase(ILoggedUsuario loggedUsuario, IUsuarioUpdateOnlyRepository repository, IUsuarioReadOnlyRepository usuarioReadOnlyRepository, IUnitOfWork unitOfWork, ISenhaEncripter senhaEncripter)
        {
            _loggedUsuario = loggedUsuario;
            _repository = repository;
            _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _senhaEncripter = senhaEncripter;
        }

        public async Task Execute(int Id, RequestUsuarioJson request)
        {
            var usuario = await _repository.GetById(Id);
            var loggedUsuario = await _loggedUsuario.Usuario();

            if (usuario == null || usuario.ClubeId != loggedUsuario.ClubeId)
            {
                throw new NotFoundException(ResourceMessagesException.USUARIO_NAO_ENCONTRADO);
            }

            await Validate(request, usuario);
            usuario.AtualizadoEm = DateTime.UtcNow;
            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.Senha = _senhaEncripter.Encrypt(request.Senha);
            usuario.CPF = CpfUtils.Format(request.CPF);
            usuario.DataNascimento = request.DataNascimento;
            usuario.EnderecoPais = request.EnderecoPais;
            usuario.EnderecoEstado = request.EnderecoEstado;
            usuario.EnderecoCidade = request.EnderecoCidade;
            usuario.EnderecoBairro = request.EnderecoBairro;
            usuario.EnderecoRua = request.EnderecoRua;
            usuario.EnderecoNumero = request.EnderecoNumero;
            usuario.CR = request.CR;
            usuario.DataVencimentoCR = request.DataVencimentoCR;
            usuario.SFPCVinculacao = request.SFPCVinculacao;
            usuario.NumeroFiliacao = request.NumeroFiliacao;
            usuario.DataFiliacao = request.DataFiliacao;
            usuario.DataRenovacaoFiliacao = request.DataRenovacaoFiliacao;

            _repository.Update(usuario);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUsuarioJson request, Domain.Entities.Usuario usuario)
        {
            var validator = new UsuarioValidator();

            var result = validator.Validate(request);


            if (!usuario.Email.Equals(request.Email))
            {
                var usuarioExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithEmail(request.Email);
                if (usuarioExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_JA_CADASTRADO));
            }

            if (!CpfUtils.Format(usuario.CPF).Equals(request.CPF))
            {
                var usuarioExistCPF = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCPF(request.CPF);
                if (usuarioExistCPF)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("cpf", ResourceMessagesException.CPF_JA_CADASTRADO));
            }

            if (!usuario.CR.Equals(request.CR))
            {
                var usuarioExistCPF = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCR(request.CPF);
                if (usuarioExistCPF)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("cr", ResourceMessagesException.CR_JA_CADASTRADO));
            }

            if (!usuario.NumeroFiliacao.Equals(request.NumeroFiliacao))
            {
                var usuarioExistCPF = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithNumeroFiliacao(request.NumeroFiliacao);
                if (usuarioExistCPF)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("numero_filiacao", ResourceMessagesException.NUMERO_FILIACAO_JA_CADASTRADO));
            }

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
