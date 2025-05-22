using System;
using LNP.BuisnessLogic;

namespace LNP.BuisnessLogic.Helper
{
    public class Hasher
    {
        public string HashPassword(string password)
        {
            using (var hashAlgorithm = System.Security.Cryptography.SHA256.Create())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashedBytes = hashAlgorithm.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}