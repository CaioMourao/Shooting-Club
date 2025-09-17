
using AutoMapper;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Token;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Cryptography;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.Register
{
    public class RegisterUsuarioUseCase : IRegisterUsuarioUseCase
    {
        private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
        private readonly IUsuarioWriteOnlyRepository _usuarioWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISenhaEncripter _passwordEncripter;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;

        public RegisterUsuarioUseCase(
            IUsuarioReadOnlyRepository usuarioReadOnlyRepository,
            IUsuarioWriteOnlyRepository usuarioWriteOnlyRepository,
            IMapper mapper,
            ISenhaEncripter passwordEncripter,
            IUnitOfWork unitOfWork,
            ILoggedUsuario loggedUsuario,
            IRefreshTokenGenerator refreshTokenGenerator,
            ITokenRepository tokenRepository
            )
        {
            _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
            _usuarioWriteOnlyRepository = usuarioWriteOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _loggedUsuario = loggedUsuario;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
        }
        public async Task<ResponseRegisteredUsuarioJson> Execute(RequestUsuarioJson request)
        {
            await Validate(request);

            var loggedUsuario = await _loggedUsuario.Usuario();
            var usuario = _mapper.Map<Domain.Entities.Usuario>(request);

            usuario.Senha = _passwordEncripter.Encrypt(request.Senha);
            usuario.IdentificadorUsuario = Guid.NewGuid();

            usuario.CPF = CpfUtils.Format(request.CPF);
            usuario.ClubeId = loggedUsuario.ClubeId;

            await _usuarioWriteOnlyRepository.Add(usuario);

            await _unitOfWork.Commit();

            var refreshToken = await CreateAndSaveRefreshToken(usuario);

            return new ResponseRegisteredUsuarioJson { 
                Nome = request.Nome
            };

        }

        public async Task<string> CreateAndSaveRefreshToken(Domain.Entities.Usuario usuario)
        {
            var refreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UsuarioId = usuario.Id
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);

            await _unitOfWork.Commit();

            return refreshToken.Value;

        }

        private async Task Validate(RequestUsuarioJson request)
        {
            var validator = new UsuarioValidator();

            var result = validator.Validate(request);

            var cpf = CpfUtils.Format(request.CPF);
            var EmailExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithEmail(request.Email);
            var CPFexist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCPF(cpf);
            var CRexist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCR(request.CR);
            var NumeroFiliacaoExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithNumeroFiliacao(request.NumeroFiliacao);

            if (EmailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_JA_CADASTRADO));

            if(CRexist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.CR_JA_CADASTRADO));

            if (CPFexist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.CPF_JA_CADASTRADO));

            if (NumeroFiliacaoExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.NUMERO_FILIACAO_JA_CADASTRADO));

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
