namespace ShootingClub.Domain.Entities
{
    public class ArmaPortePessoal : ArmaBase
    {
        public string NumeroCertificado { get; set; } = string.Empty;
        public DateOnly DataCertificacao { get; set; }
        public DateOnly ValidadeCertificacao { get; set; }
    }
}
