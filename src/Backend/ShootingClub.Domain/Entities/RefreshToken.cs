namespace ShootingClub.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public required string Value { get; set; } = string.Empty;
        public required int UsuarioId { get; set; }
        public Usuario usuario { get; set; } = default!;
    }
}
