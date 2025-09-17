using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Filters;

namespace ShootingClub.API.Attributes
{
    public class AuthenticatedAdminAttribute : TypeFilterAttribute
    {
        public AuthenticatedAdminAttribute() : base(typeof(AuthenticatedAdminFilter))
        {
        }
    }
}
