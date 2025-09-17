using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Filters;

namespace ShootingClub.API.Attributes
{
    public class AuthenticatedAdminWithClubeAttribute : TypeFilterAttribute
    {
        public AuthenticatedAdminWithClubeAttribute() : base(typeof(AuthenticatedAdminWithClubeFilter))
        {
        }
    }
}
