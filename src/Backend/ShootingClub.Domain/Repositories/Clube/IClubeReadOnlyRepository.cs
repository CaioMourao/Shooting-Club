namespace ShootingClub.Domain.Repositories.Clube
{
    public interface IClubeReadOnlyRepository
    {
        public Task<Entities.Clube> GetById(int id);
        public Task<bool> ExistActiveClubeWithAdmin(int responsavelId);
        public Task<bool> ExistActiveClubeWithCNPJ(string cnpj);
        public Task<bool> ExistActiveClubeWithCertificadoRegistro(string certificadoRegistro);
    }
}
