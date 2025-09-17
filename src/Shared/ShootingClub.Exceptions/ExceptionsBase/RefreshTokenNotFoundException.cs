namespace ShootingClub.Exceptions.ExceptionsBase
{
    public class RefreshTokenNotFoundException : ShootingClubException
    {
        public RefreshTokenNotFoundException(): base(ResourceMessagesException.USUARIO_NAO_ENCONTRADO)
        {
        }
    }
}
