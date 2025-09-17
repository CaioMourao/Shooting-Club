using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using ShootingClub.API.Filters;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.API.Attributes
{
    public class AuthenticatedUsuarioAttribute : TypeFilterAttribute
    {
        public AuthenticatedUsuarioAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }

}
