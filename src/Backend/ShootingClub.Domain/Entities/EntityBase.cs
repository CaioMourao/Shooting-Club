namespace ShootingClub.Domain.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}
