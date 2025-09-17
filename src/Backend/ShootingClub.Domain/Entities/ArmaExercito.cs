namespace ShootingClub.Domain.Entities
{
    public class ArmaExercito : ArmaBase
    {
        public string NumeroSigma { get; set; } = string.Empty;
        public string LocalRegistro { get; set; } = string.Empty;
        public DateOnly DataExpedicaoCRAF { get; set; }
        public DateOnly ValidadeCRAF { get; set; }
        public string NumeroGTE { get; set; } = string.Empty;
        public DateOnly ValidadeGTE { get; set; }
    }
}
