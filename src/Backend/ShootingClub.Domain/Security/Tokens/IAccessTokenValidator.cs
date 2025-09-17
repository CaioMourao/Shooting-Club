namespace ShootingClub.Domain.Security.Tokens
{
    public interface IAccessTokenValidator
    {
        public (Guid IdentificadorUsuario, int NivelUsuario) ValidateAndGetIdentificadorUsuarioAndNivel(string token);
    }
}
