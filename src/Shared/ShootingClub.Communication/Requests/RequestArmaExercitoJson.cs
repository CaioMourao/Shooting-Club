using ShootingClub.Communication.Enums;

namespace ShootingClub.Communication.Requests
{
    public class RequestArmaExercitoJson : RequestArmaBaseJson
    {
        public RequestArmaExercitoJson() => this.TipoPosse = TipoPosseArma.Exercito;
        public string? NumeroSigma { get; set; }
        public string? LocalRegistro { get; set; }
        public DateOnly? DataExpedicaoCRAF { get; set; }
        public DateOnly? ValidadeCRAF { get; set; }
        public string? NumeroGTE { get; set; }
        public DateOnly? ValidadeGTE { get; set; }
    }
}
