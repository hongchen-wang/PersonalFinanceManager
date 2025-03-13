using System.Security.Cryptography;

namespace PersonalFinanceManager.Server.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
