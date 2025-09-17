using Microsoft.AspNetCore.Mvc.Filters;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.API.Filters
{
    public class AuthenticatedAdminWithClubeFilter : AuthenticatedBaseFilter, IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUsuarioReadOnlyRepository _repository;

        public AuthenticatedAdminWithClubeFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnlyRepository repository)
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
            var hasClube = await _repository.ActiveUsuarioHasClube(identificador);

            if (!exist)
                throw new ShootingClubException(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO);

            if (!hasClube)
                throw new ShootingClubException(ResourceMessagesException.ADMIN_NAO_POSSUI_CLUBE_CADASTRADO);
        }
    }
}
