using ShootingClub.Domain.Enums;

namespace ShootingClub.Domain.Dtos
{
    public record FilterArmasDto
    {
        public TipoPosseArma? TipoPosse { get; init; }
        public string? Tipo { get; init; }
        public string? Marca { get; init; }
        public string? Calibre { get; set; }
        public string? NumeroSerie { get; init; }
        public bool? ProximoExpiracao { get; init; }
        public bool? SoArmasDoClube { get; init; }
        public int? IdUsuario { get; init; }
    }
}
