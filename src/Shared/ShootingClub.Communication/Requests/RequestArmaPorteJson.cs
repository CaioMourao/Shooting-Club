using ShootingClub.Communication.Enums;

namespace ShootingClub.Communication.Requests
{
    public class RequestArmaPorteJson : RequestArmaBaseJson
    {
        public RequestArmaPorteJson() => this.TipoPosse = TipoPosseArma.PortePessoal;
        public string? NumeroCertificado { get; set; }
        public DateOnly? DataCertificacao { get; set; }
        public DateOnly? ValidadeCertificacao { get; set; }
    }
}
