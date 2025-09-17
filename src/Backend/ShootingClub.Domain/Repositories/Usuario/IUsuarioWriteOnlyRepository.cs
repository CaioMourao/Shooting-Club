namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioWriteOnlyRepository
    {
        public Task Add(Entities.Usuario usuario);
        public Task Delete(int UsuarioId);
    }
}
