
namespace WebCheckoutPlacetopay.Models
{
    public class APIAuth
    {
        public string login { get; private set; }
        public string tranKey { get; private set; }
        public string nonce { get; private set; }
        public string seed { get; private set; }
        public string secretKey { get; private set; }

        public APIAuth(string login, string secretKey, string nonce, string tranKey, string seed)
        {
            this.login = login;
            this.secretKey = secretKey;
            this.nonce = nonce;
            this.tranKey = tranKey;
            this.seed = seed;
        }
    }
}