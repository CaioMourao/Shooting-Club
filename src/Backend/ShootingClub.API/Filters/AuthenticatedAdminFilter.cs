using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions.ExceptionsBase;
using ShootingClub.Exceptions;

namespace ShootingClub.API.Filters
{
    public class AuthenticatedAdminFilter : AuthenticatedBaseFilter, IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUsuarioReadOnlyRepository _repository;

        public AuthenticatedAdminFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }

        protected override async Task AuthorizeAsync(string token, AuthorizationFilterContext context)
        {
            var (identificador, nivel) = _accessTokenValidator.ValidateAndGetIdentificadorUsuarioAndNivel(token);

            if (nivel != (int)Domain.Enums.NivelUsuario.AdminUsuario)
            {
                throw new ShootingClubException(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO);
            }

            var exist = await _repository.ExistActiveUserWithIdentificador(identificador);

            if (!exist)
            {
                throw new ShootingClubException(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO);
            }
        }
    }
}
