namespace ShootingClub.Communication.Responses
{
    public class ResponseArmaPortePessoalJson : ResponseArmaBaseJson
    {
        public string? NumeroCertificado { get; set; }
        public DateOnly? DataCertificacao { get; set; }
        public DateOnly? ValidadeCertificacao { get; set; }
    }
}
