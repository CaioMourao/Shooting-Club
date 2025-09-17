using ShootingClub.Domain.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Infrastructure.Security.Cryptography
{
    public class BCryptNet : ISenhaEncripter
    {
        public string Encrypt(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool IsValid(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
