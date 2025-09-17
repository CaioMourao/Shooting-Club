namespace ShootingClub.Domain.Repositories.Clube
{
    public interface IClubeUpdateOnlyRepository
    {
        public Task<Entities.Clube> GetById(int id);
        public void Update(Entities.Clube clube);
    }
}
