using ShootingClub.Domain.Entities;

namespace ShootingClub.Domain.Services.LoggedUsuario
{
    public interface ILoggedUsuario
    {
        public Task<Usuario> Usuario();
    }
}
