using ShootingClub.Domain.Dtos;

namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioReadOnlyRepository
    {
        public Task<bool> ExistActiveUsuarioWithEmail(string email);
        public Task<bool> ExistActiveUsuarioWithCPF(string cpf);
        public Task<bool> ExistActiveUsuarioWithCR(string cr);
        public Task<bool> ExistActiveUsuarioWithClubeAndCPF(int clubeId, string cpf);
        public Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao);
        public Task<bool> ActiveUsuarioHasClube(Guid IdentificadorUsuario);
        public Task<Entities.Usuario?> GetByEmail(string email);
        public Task<int> GetIdUsuarioByCPF(string cpf);
        public Task<bool> ExistActiveUserWithIdentificador(Guid IdentificadorUsuario);
        public Task<Entities.Usuario> GetById(int id);
        public Task<bool> CanDelete(Entities.Usuario admin, int UsuarioId);
        public Task<IList<Entities.Usuario>> Filter(Entities.Usuario admin, FilterUsuariosDto filters);
        public Task<int> CountTotalByClube(int clubeId);
        public Task<int> CountUsuariosRegisteredInTheLastYear(Entities.Usuario admin);
    }
}
