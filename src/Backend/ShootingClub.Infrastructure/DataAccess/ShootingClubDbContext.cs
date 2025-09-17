using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Enums;
using System.Reflection;

namespace ShootingClub.Infrastructure.DataAccess
{
    public class ShootingClubDbContext : DbContext
    {
        public ShootingClubDbContext(DbContextOptions<ShootingClubDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Clube> Clubes { get; set; }

        public DbSet<ArmaBase> Armas { get; set; }
        public DbSet<ArmaExercito> ArmasExercito { get; set; }
        public DbSet<ArmaPF> ArmasPoliciaFederal { get; set; }
        public DbSet<ArmaPortePessoal> ArmasPortePessoal { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShootingClubDbContext).Assembly);

            modelBuilder.Entity<ArmaBase>().UseTptMappingStrategy();
            modelBuilder.Entity<ArmaExercito>().ToTable("ArmasExercito");
            modelBuilder.Entity<ArmaPF>().ToTable("ArmasPoliciaFederal");
            modelBuilder.Entity<ArmaPortePessoal>().ToTable("ArmasPortePessoal");
        }
    }
}