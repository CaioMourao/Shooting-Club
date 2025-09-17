namespace ShootingClub.Domain.Security.Cryptography
{
    public interface ISenhaEncripter
    {
        public string Encrypt(string senha);
        public bool IsValid(string password, string passwordHash);
    }
}
