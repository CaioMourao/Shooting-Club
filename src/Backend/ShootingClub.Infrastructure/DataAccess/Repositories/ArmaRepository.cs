using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Enums;
using ShootingClub.Domain.Dtos;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class ArmaRepository : IArmaWriteOnlyRepository, IArmaReadOnlyRepository, IArmaUpdateOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public ArmaRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(ArmaBase arma) => await _dbContext.Armas.AddAsync(arma);

        public async Task<bool> ExistActiveArmaWithNumeroSerie(string numeroSerie)
        {
            return await _dbContext.Armas.AnyAsync(arma => arma.NumeroSerie.Equals(numeroSerie) && arma.Ativo);
        }

        public async Task<bool> ExistActiveArmaWithNumeroSerieAndIgnoreId(string numeroSerie, int armaId)
        {
            return await _dbContext.Armas.AnyAsync(arma => arma.Ativo && arma.NumeroSerie.Equals(numeroSerie) && arma.Id != armaId);
        }

        public async Task<IList<ArmaBase>> Filter(Usuario usuario, FilterArmasDto filters)
        {
            IQueryable<ArmaBase> query = _dbContext.Armas.AsNoTracking().Where(arma => arma.Ativo);

            if (usuario.Nivel == NivelUsuario.AdminUsuario)
            {

                query = query.Where(arma => (arma.UsuarioId == 0 && arma.ClubeId == usuario.ClubeId) ||
                                             (_dbContext.Usuarios.Any(u => u.Id == arma.UsuarioId && u.ClubeId == usuario.ClubeId)));
            }
            else
            {
                query = query.Where(arma => arma.UsuarioId == usuario.Id);
            }

            if (filters.IdUsuario.HasValue)
            {
                query = query.Where(a => a.UsuarioId == filters.IdUsuario.Value);
            }

            if (filters.SoArmasDoClube == true)
            {
                query = query.Where(a => a.UsuarioId == 0 && a.ClubeId == usuario.ClubeId);
            }

            if (filters.TipoPosse.HasValue)
            {
                query = query.Where(arma => arma.TipoPosse == filters.TipoPosse.Value);
            }

            if (!string.IsNullOrWhiteSpace(filters.Tipo))
            {
                query = query.Where(arma => arma.Tipo.Contains(filters.Tipo));
            }

            if (!string.IsNullOrWhiteSpace(filters.Marca))
            {
                query = query.Where(arma => arma.Marca.Contains(filters.Marca));
            }

            if (!string.IsNullOrWhiteSpace(filters.Calibre))
            {
                query = query.Where(arma => !string.IsNullOrEmpty(arma.Calibre) && arma.Calibre.Contains(filters.Calibre));
            }

            if (!string.IsNullOrWhiteSpace(filters.NumeroSerie))
            {
                query = query.Where(arma => arma.NumeroSerie.Contains(filters.NumeroSerie));
            }

            if (filters.ProximoExpiracao == true)
            {
                var dataLimite = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

                var armasExercitoExpirando = query.OfType<ArmaExercito>()
                    .Where(ae => ae.ValidadeCRAF <= dataLimite || ae.ValidadeGTE <= dataLimite);

                var armasPfExpirando = query.OfType<ArmaPF>()
                    .Where(apf => apf.DataValidadePF <= dataLimite);

                var armasPorteExpirando = query.OfType<ArmaPortePessoal>()
                    .Where(app => app.ValidadeCertificacao <= dataLimite);

                query = armasExercitoExpirando.AsQueryable<ArmaBase>()
                    .Concat(armasPfExpirando)
                    .Concat(armasPorteExpirando);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CanDelete(Usuario usuario, int armaId)
        {
            if (usuario.Nivel != NivelUsuario.AdminUsuario)
            {
                return false;
            }

            return await _dbContext.Armas.AnyAsync(arma => arma.Id == armaId && arma.Ativo &&
                ((arma.UsuarioId == 0 && arma.ClubeId == usuario.ClubeId) ||
                (_dbContext.Usuarios.Any(u => u.Id == arma.UsuarioId && u.ClubeId == usuario.ClubeId))));
        }

        private async Task<ArmaBase?> GetByIdPrivado(Usuario usuario, int armaId, bool asNoTracking)
        {
            var query = asNoTracking ? _dbContext.Armas.AsNoTracking() : _dbContext.Armas.AsQueryable();

            var arma = await query.FirstOrDefaultAsync(a => a.Id == armaId && a.Ativo);

            if (arma == null)
            {
                return null;
            }

            if (usuario.Nivel == NivelUsuario.AdminUsuario)
            {
                bool pertenceAoClube = (arma.UsuarioId == 0 && arma.ClubeId == usuario.ClubeId) ||
                                       await _dbContext.Usuarios.AnyAsync(u => u.Id == arma.UsuarioId && u.ClubeId == usuario.ClubeId);
                return pertenceAoClube ? arma : null;
            }

            return arma.UsuarioId == usuario.Id ? arma : null;
        }

        async Task<ArmaBase?> IArmaReadOnlyRepository.GetById(Usuario usuario, int armaId)
        {
            return await GetByIdPrivado(usuario, armaId, asNoTracking: true);
        }

        async Task<ArmaBase?> IArmaUpdateOnlyRepository.GetById(Usuario usuario, int armaId)
        {
            return await GetByIdPrivado(usuario, armaId, asNoTracking: false);
        }

        public async Task Delete(int armaId)
        {
            var arma = await _dbContext.Armas.FindAsync(armaId);
            if (arma != null)
            {
                _dbContext.Armas.Remove(arma);
            }
        }

        public void Update(ArmaBase arma) => _dbContext.Armas.Update(arma);

        public async Task<int> CountExpiredByClub(int clubeId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userIdsDoClube = await _dbContext.Usuarios
                .Where(u => u.ClubeId == clubeId)
                .Select(u => u.Id)
                .ToListAsync();

            var baseQuery = _dbContext.Armas
                .Where(a =>
                    (a.UsuarioId == 0 && a.ClubeId == clubeId) ||
                    (a.UsuarioId != 0 && userIdsDoClube.Contains(a.UsuarioId))
                );

            var armasExercitoExpirando = await baseQuery.OfType<ArmaExercito>()
                .CountAsync(ae => ae.ValidadeCRAF < today || ae.ValidadeGTE < today);

            var armasPfExpirando = await baseQuery.OfType<ArmaPF>()
                .CountAsync(apf => apf.DataValidadePF < today);

            var armasPorteExpirando = await baseQuery.OfType<ArmaPortePessoal>()
                .CountAsync(app => app.ValidadeCertificacao < today);

            return armasExercitoExpirando + armasPfExpirando + armasPorteExpirando;
        }

        public async Task<int> CountTotalByClub(int clubeId)
        {
            var userIdsDoClube = await _dbContext.Usuarios
            .Where(u => u.ClubeId == clubeId)
            .Select(u => u.Id)
            .ToListAsync();

            return await _dbContext.Armas
                .Where(a =>
                    (a.UsuarioId == 0 && a.ClubeId == clubeId) ||
                    (a.UsuarioId != 0 && userIdsDoClube.Contains(a.UsuarioId))
                )
                .CountAsync();
        }

        public async Task<int> CountArmasRegisteredInTheLastYear(int clubeId)
        {
            var userIdsDoClube = await _dbContext.Usuarios
            .Where(u => u.ClubeId == clubeId)
            .Select(u => u.Id)
            .ToListAsync();

            var lastYear = DateTime.UtcNow.AddYears(-1);

            return await _dbContext.Armas
                .Where(a =>
                    ((a.UsuarioId == 0 && a.ClubeId == clubeId) ||
                    (a.UsuarioId != 0 && userIdsDoClube.Contains(a.UsuarioId)))
                    && a.CriadoEm >= lastYear
                )
                .CountAsync();
        }
    }
}