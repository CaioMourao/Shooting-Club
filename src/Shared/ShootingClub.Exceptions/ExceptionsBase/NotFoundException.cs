namespace ShootingClub.Exceptions.ExceptionsBase
{
    public class NotFoundException : ShootingClubException
    {
        public NotFoundException(string message) : base (message)
        {  
        }
    }
}
