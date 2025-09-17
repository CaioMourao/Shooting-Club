using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Token;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Cryptography;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions.ExceptionsBase;
using System.Reflection.Metadata;

namespace ShootingClub.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {

        private readonly IUsuarioReadOnlyRepository _repository;
        private readonly ISenhaEncripter _passwordEncripter;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        public DoLoginUseCase(IUsuarioReadOnlyRepository repository, ISenhaEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IUnitOfWork unitOfWork, ITokenRepository tokenRepository)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
            _tokenRepository = tokenRepository;
        }
        public async Task<ResponseLoggedInUsuarioJson> Execute(RequestLoginJson request)
        {
            var usuario = await _repository.GetByEmail(request.Email);

            if(usuario is null || !_passwordEncripter.IsValid(request.Senha, usuario.Senha))
                throw new InvalidLoginException();

            var refreshToken = await CreateAndSaveRefreshToken(usuario);

            return new ResponseLoggedInUsuarioJson
            {
                Nome = usuario.Nome,
                Email = request.Email,
                Tokens = new ResponseTokensJson 
                { 
                    AccessToken = _accessTokenGenerator.Generate(usuario.IdentificadorUsuario, (int)usuario.Nivel),
                    RefreshToken = refreshToken
                }
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
    }
}
