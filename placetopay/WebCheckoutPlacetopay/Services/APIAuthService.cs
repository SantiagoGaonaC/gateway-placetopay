using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using WebCheckoutPlacetopay.Models;

namespace WebCheckoutPlacetopay.Services
{
    public class APIAuthService
    {
        public APIAuth GenerateAPIAuth(string login, string secretKey)
        {
            string nonce = GenerateNonce();
            string seed = GenerateSeed();
            string tranKey = GenerateTranKey(nonce, seed, secretKey);
            return new APIAuth(login, secretKey, nonce, tranKey, seed);
        }

        private string GenerateNonce()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] nonceBytes = new byte[16];
                rng.GetBytes(nonceBytes);
                return Convert.ToBase64String(nonceBytes);
            }
        }

        private string GenerateSeed()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private string GenerateTranKey(string nonce, string seed, string secretKey)
        {
            using (var sha1 = SHA1.Create())
            {
                var tranKeyInput = Convert.FromBase64String(nonce)
                    .Concat(Encoding.UTF8.GetBytes(seed))
                    .Concat(Encoding.UTF8.GetBytes(secretKey))
                    .ToArray();
                byte[] tranKeyRaw = sha1.ComputeHash(tranKeyInput);
                return Convert.ToBase64String(tranKeyRaw);
            }
        }
    }
}
