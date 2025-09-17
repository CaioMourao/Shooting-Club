using ShootingClub.Domain.Dtos;

namespace ShootingClub.Domain.Repositories.Arma
{
    public interface IArmaReadOnlyRepository
    {
        public Task<bool> ExistActiveArmaWithNumeroSerie(string numeroSerie);
        public Task<IList<Entities.ArmaBase>> Filter(Entities.Usuario usuario, FilterArmasDto filters);
        public Task<Entities.ArmaBase?> GetById(Entities.Usuario usuario, int armaId);
        public Task<bool> CanDelete(Entities.Usuario usuario, int ArmaId);
        public Task<int> CountExpiredByClub(int clubeId);
        public Task<int> CountTotalByClub(int clubeId);
        public Task<int> CountArmasRegisteredInTheLastYear(int clubeId);
    }
}
