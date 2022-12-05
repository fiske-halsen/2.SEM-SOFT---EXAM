using Common.Dto;
using GraphqlDemo.Models;
using IdentityModel.Client;

namespace GraphqlDemo.Services
{
    public interface ITokenService
    {
        public Task<TokenResult> RequestTokenClientFromInternalMicroService(ApplicationCredentials applicationCredentials);
        public Task<TokenResult> RequestTokenForUser(string userName, string password);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _identityServerUrl;


        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _identityServerUrl = configuration["IdentityServer:Host"];
        }

        public async Task<TokenResult> RequestTokenClientFromInternalMicroService(ApplicationCredentials applicationCredentials)
        {
            using HttpClient client = new HttpClient();

            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(_identityServerUrl);

            if (disco.IsError)
            {
                return new TokenResult { IsSucces = false, Error = disco.Error };
            }

            TokenResponse token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = applicationCredentials.ClientId,
                ClientSecret = applicationCredentials.ClientSecret,
                Scope = applicationCredentials.Scope
            });

            return new TokenResult { IsSucces = !token.IsError, TokenResponse = token };
        }

        public async Task<TokenResult> RequestTokenForUser(string userName, string password)
        {
            using HttpClient client = new HttpClient();

            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(_identityServerUrl);

            if (disco.IsError)
            {
                return new TokenResult { IsSucces = false, Error = disco.Error };
            }
            else
            {
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "Gateway",
                    ClientSecret = _configuration["Gateway:Key"],
                    UserName = userName,
                    Password = password,
                    Scope = "Gateway",
                });

                return new TokenResult { IsSucces = !tokenResponse.IsError, TokenResponse = tokenResponse };
            }

        }
    }
}
