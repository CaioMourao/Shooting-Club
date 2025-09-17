namespace ShootingClub.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
