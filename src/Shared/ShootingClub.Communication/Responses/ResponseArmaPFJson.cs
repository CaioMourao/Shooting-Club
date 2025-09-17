namespace ShootingClub.Communication.Responses
{
    public class ResponseArmaPFJson : ResponseArmaBaseJson
    {
        public string? NumeroSinarm { get; set; }
        public string? NumeroRegistroPF { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public DateOnly? DataValidadePF { get; set; }
    }
}
