using Microsoft.AspNetCore.Mvc.Filters;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.API.Filters
{
    public class AuthenticatedUserFilter : AuthenticatedBaseFilter, IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUsuarioReadOnlyRepository _repository;
        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }
        protected override async Task AuthorizeAsync(string token, AuthorizationFilterContext context)
        {
            var (identificador, _) = _accessTokenValidator.ValidateAndGetIdentificadorUsuarioAndNivel(token);
            var exist = await _repository.ExistActiveUserWithIdentificador(identificador);

            if (!exist)
            {
                throw new ShootingClubException(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO);
            }
        }

    }
}
