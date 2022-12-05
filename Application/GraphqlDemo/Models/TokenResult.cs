using IdentityModel.Client;

namespace GraphqlDemo.Models
{
    public class TokenResult
    {
        public bool IsSucces { get; set; }
        public TokenResponse TokenResponse { get; set; }
        public string Error { get; set; }
    }
}