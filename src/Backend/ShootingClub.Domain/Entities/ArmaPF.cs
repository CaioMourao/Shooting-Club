namespace ShootingClub.Domain.Entities
{
    public class ArmaPF : ArmaBase
    {
        public string NumeroSinarm { get; set; } = string.Empty;
        public string NumeroRegistroPF { get; set; } = string.Empty;
        public string NumeroNotaFiscal { get; set; } = string.Empty;
        public DateOnly DataValidadePF { get; set; }
    }
}
