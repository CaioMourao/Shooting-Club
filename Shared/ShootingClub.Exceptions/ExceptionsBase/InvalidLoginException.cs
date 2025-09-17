namespace ShootingClub.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : ShootingClubException
    {
        public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OU_SENHA_INVALIDO)
        {
        }
    }
}
