using ShootingClub.Communication.Enums;

namespace ShootingClub.Communication.Requests
{
    public class RequestFilterArmaJson
    {
        public TipoPosseArma? TipoPosse { get; set; }
        public string? Tipo { get; set; }
        public string? Marca { get; set; }
        public string? Calibre { get; set; }
        public string? NumeroSerie { get; set; }
        public bool? ProximoExpiracao { get; set; }
        public bool? SoArmasDoClube { get; set; }
        public int? IdUsuario { get; set; }
    }
}
