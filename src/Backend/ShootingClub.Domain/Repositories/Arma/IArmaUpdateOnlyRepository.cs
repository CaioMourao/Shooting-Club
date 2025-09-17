using ShootingClub.Domain.Entities;

namespace ShootingClub.Domain.Repositories.Arma
{
    public interface IArmaUpdateOnlyRepository
    {
        public Task<Entities.ArmaBase?> GetById(Entities.Usuario usuario, int armaId);
        public Task<bool> ExistActiveArmaWithNumeroSerieAndIgnoreId(string NumeroSerie, int Armaid);
        public void Update(Entities.ArmaBase arma);
    }
}
