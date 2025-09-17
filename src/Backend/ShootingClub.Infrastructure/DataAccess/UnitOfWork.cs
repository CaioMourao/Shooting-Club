using ShootingClub.Domain.Repositories;

namespace ShootingClub.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShootingClubDbContext _dbContext;

        public UnitOfWork(ShootingClubDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
