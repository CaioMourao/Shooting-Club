using ShootingClub.Communication.Enums;

namespace ShootingClub.Communication.Requests
{
    public class RequestArmaPFJson : RequestArmaBaseJson
    {
        public RequestArmaPFJson() => this.TipoPosse = TipoPosseArma.PoliciaFederal;
        public string? NumeroSinarm { get; set; }
        public string? NumeroRegistroPF { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public DateOnly? DataValidadePF { get; set; }
    }
}
