namespace ShootingClub.Communication.Requests
{
    public class RequestFilterUsuarioJson
    {
        public string? Nome { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? CPF { get; set; } = string.Empty;
        public bool? ProximoExpiracao { get; set; }
    }
}
