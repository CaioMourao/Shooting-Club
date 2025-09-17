namespace ShootingClub.Domain.Repositories.Clube
{
    public interface IClubeWriteOnlyRepository
    {
        public Task Add(Entities.Clube clube);
    }
}
