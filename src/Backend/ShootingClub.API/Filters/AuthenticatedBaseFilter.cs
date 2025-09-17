using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.API.Filters
{
    public abstract class AuthenticatedBaseFilter
    {
        protected string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authorization = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorization))
            {
                throw new ShootingClubException(ResourceMessagesException.SEM_TOKEN);
            }

            return authorization["Bearer ".Length..].Trim();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);
                await AuthorizeAsync(token, context);
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
                {
                    TokenIsExpired = true,
                });
            }
            catch (ShootingClubException ex)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO));
            }
        }

        protected abstract Task AuthorizeAsync(string token, AuthorizationFilterContext context);
    }
}
