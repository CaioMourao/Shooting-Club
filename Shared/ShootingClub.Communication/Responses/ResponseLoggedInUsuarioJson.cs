namespace ShootingClub.Communication.Responses
{
    public class ResponseLoggedInUsuarioJson
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ResponseTokensJson Tokens { get; set; } = default!;
    }
}
