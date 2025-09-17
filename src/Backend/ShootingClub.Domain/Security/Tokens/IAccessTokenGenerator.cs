namespace ShootingClub.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        public string Generate(Guid identificadorUsuario, int nivelUsuario);
    }
}
