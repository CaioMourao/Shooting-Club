namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioUpdateOnlyRepository
    {
        public Task<Entities.Usuario> GetById(int id);
        public void Update(Entities.Usuario user);
    }
}
