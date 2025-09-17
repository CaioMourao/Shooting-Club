using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Dtos;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Usuario;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioWriteOnlyRepository, IUsuarioReadOnlyRepository, IUsuarioUpdateOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public UsuarioRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;


        public async Task Add(Usuario usuario) => await _dbContext.Usuarios.AddAsync(usuario);

        public async Task<bool> ExistActiveUsuarioWithEmail(string email)
        {
           return await _dbContext.Usuarios.AnyAsync(usuario => usuario.Email.Equals(email) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithCPF(string cpf)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.CPF.Equals(cpf) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithClubeAndCPF(int clubeId, string cpf)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.CPF.Equals(cpf) && usuario.ClubeId.Equals(clubeId) && usuario.Ativo); 
        }

        public async Task<bool> ExistActiveUsuarioWithCR(string cr)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => !string.IsNullOrEmpty(usuario.CR) && usuario.CR.Equals(cr) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.NumeroFiliacao.Equals(numeroFiliacao) && usuario.Ativo);
        }


        public async Task<Usuario?> GetByEmail(string email)
        {
            return await _dbContext
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(usuario =>usuario.Ativo && usuario.Email.Equals(email));
        }

        public async Task<int> GetIdUsuarioByCPF(string cpf)
        {
            return await _dbContext
                .Usuarios
                .Where(usuario => usuario.CPF.Equals(cpf) && usuario.Ativo)
                .Select(usuario => usuario.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistActiveUserWithIdentificador(Guid IdentificadorUsuario) => await _dbContext.Usuarios.AnyAsync(usuario => usuario.IdentificadorUsuario.Equals(IdentificadorUsuario) && usuario.Ativo);

        async Task<Usuario> IUsuarioUpdateOnlyRepository.GetById(int id)
        {
            return await _dbContext
                .Usuarios
                .FirstAsync(usuario => usuario.Id == id);
        }
        async Task<Usuario> IUsuarioReadOnlyRepository.GetById(int id)
        {
            return await _dbContext
                .Usuarios
                .AsNoTracking()
                .FirstAsync(usuario => usuario.Id == id);
        }
        public async Task<Usuario?> GetActiveUserByIdentificador(Guid identificadorUsuario)
        {
            return await _dbContext.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdentificadorUsuario == identificadorUsuario && u.Ativo);
        }

        public async Task<bool> ActiveUsuarioHasClube(Guid IdentificadorUsuario)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.IdentificadorUsuario.Equals(IdentificadorUsuario) && usuario.Ativo && usuario.ClubeId > 0);
        }
        public void Update(Usuario usuario) => _dbContext.Usuarios.Update(usuario);


        public async Task<IList<Usuario>> Filter(Usuario admin, FilterUsuariosDto filters)
        {
            IQueryable<Usuario> query = _dbContext.Usuarios.AsNoTracking().Where(usuario => usuario.Ativo && usuario.ClubeId == admin.ClubeId && usuario.Id != admin.Id);

            if (!string.IsNullOrWhiteSpace(filters.Nome))
            {
                query = query.Where(u => u.Nome.Contains(filters.Nome));
            }

            if (!string.IsNullOrWhiteSpace(filters.Email))
            {
                query = query.Where(u => u.Email.Contains(filters.Email));
            }

            if (!string.IsNullOrWhiteSpace(filters.CPF))
            {
                query = query.Where(u => u.CPF.Contains(filters.CPF));
            }

            if (filters.ProximoExpiracao == true)
            {
                var dataLimite = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

                query = query.Where(u => (u.DataVencimentoCR.HasValue && u.DataVencimentoCR <= dataLimite) || u.DataRenovacaoFiliacao > dataLimite);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountTotalByClube(int clubeId)
        {
            return await _dbContext.Usuarios
            .Where(u => u.ClubeId == clubeId)
            .CountAsync() - 1;
        }

        public async Task<int> CountUsuariosRegisteredInTheLastYear(Usuario admin)
        {
            var lastYear = DateTime.UtcNow.AddYears(-1);

            return await _dbContext.Usuarios.Where(u => u.ClubeId == admin.ClubeId && u.CriadoEm >= lastYear && u.Id != admin.Id).CountAsync();
        }

        public async Task<bool> CanDelete(Usuario admin, int UsuarioId) => await _dbContext.Usuarios.AnyAsync(usuario => usuario.Id == UsuarioId && usuario.ClubeId == admin.ClubeId);


        public async Task Delete(int usuarioId)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(usuarioId);
            if (usuario is null)
            {
                return;
            }
            var ArmasDoUsuario = _dbContext.Armas.Where(arma => arma.UsuarioId == usuario.Id);
            var refreshTokensDoUsuario = _dbContext.RefreshTokens.Where(token => token.UsuarioId == usuario.Id);
            
            _dbContext.RemoveRange(ArmasDoUsuario);
            _dbContext.RemoveRange(refreshTokensDoUsuario);
            _dbContext.Remove(usuario);
    
        }
    }
}
