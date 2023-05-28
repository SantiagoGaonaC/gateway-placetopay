using System.Security.Cryptography;
using System.Text;
using WebCheckoutPlacetopay.Models;

namespace WebCheckoutPlacetopay.Services
{
    public class APIAuthService
    {
        public APIAuth GenerateAPIAuth(string login, string secretKey)
        {
            // Generación del nonce, la semilla y la clave de transacción
            string nonce = GenerateNonce();
            string seed = GenerateSeed();
            string tranKey = GenerateTranKey(nonce, seed, secretKey);
            // Retorno del objeto de autenticación con todos los datos necesarios
            return new APIAuth(login, secretKey, nonce, tranKey, seed);
        }
        private string GenerateNonce()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] nonceBytes = new byte[16];
                rng.GetBytes(nonceBytes);
                return Convert.ToBase64String(nonceBytes);
            }
        }
        private string GenerateSeed()
        {
            // Genera una semilla (seed) en formato de fecha y hora actual en UTC, con el formato especificado que usa Placetopay.
            // Esta semilla se utiliza luego en el proceso de autenticación para la generación de la clave de transacción (tranKey).
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private string GenerateTranKey(string nonce, string seed, string secretKey)
        {
            using (var sha1 = SHA1.Create())
            {
                // Concatena el nonce, la semilla (seed) y la clave secreta, todos en formato de bytes.
                // El nonce se encuentra ya en formato base64, así que se convierte de base64 a bytes.
                // La semilla y la clave secreta están en formato de texto, así que se convierten a bytes usando UTF8.
                var tranKeyInput = Convert.FromBase64String(nonce)
                    .Concat(Encoding.UTF8.GetBytes(seed))
                    .Concat(Encoding.UTF8.GetBytes(secretKey))
                    .ToArray();
                // Calcula el hash SHA1 del input, esto proporciona un nivel adicional de seguridad.
                byte[] tranKeyRaw = sha1.ComputeHash(tranKeyInput);
                // Convierte el hash (que está en formato de bytes) a una cadena base64 y retorna esta cadena.
                // La cadena base64 se utiliza porque es un formato ampliamente utilizado para representar datos binarios.
                return Convert.ToBase64String(tranKeyRaw);
            }
        }
    }
}
