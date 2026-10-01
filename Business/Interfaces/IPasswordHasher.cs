using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }

    public class Sha256PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
