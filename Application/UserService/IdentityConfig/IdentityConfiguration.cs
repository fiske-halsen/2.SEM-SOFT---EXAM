using IdentityServer4.Models;
using UserService.Services;

namespace UserService.IdentityConfig
{
    /// <summary>
    /// In memory used identity configuration, should actually have a resource store for best practices
    /// However this is good for now.
    /// </summary>
    public static class IdentityConfiguration
    {
        #region Identity Config

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("FeedbackService", "FeedbackService Scope"),
                new ApiScope("Gateway", "Gateway Scope"),
                new ApiScope("DeliveryService", "DeliveryService Scope"),
                new ApiScope("OrderService", "OrderService Scope"),
                new ApiScope("RestaurantService", "DeliveryService Scope"),
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var identityConfigKeys = Helpers.GetIdentityConfigKeys(configuration);

            return new List<Client>
            {
                new Client
                {
                    ClientId = "FeedbackService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"FeedbackService"},
                    ClientSecrets = {new Secret(identityConfigKeys.FeedbackServiceKey.Sha256())},
                    AllowAccessTokensViaBrowser = true,
                },

                new Client
                {
                    ClientId = "Gateway",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes =
                        GrantTypes.ResourceOwnerPassword, // Notice the resource owner password flow here
                    AllowedScopes = {"UserService"},
                    ClientSecrets = {new Secret(identityConfigKeys.GatewayKey.Sha256())},
                    AllowAccessTokensViaBrowser = true,
                },

                new Client
                {
                    ClientId = "DeliveryService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"DeliveryService"},
                    ClientSecrets = {new Secret(identityConfigKeys.DeliveryServiceKey.Sha256())},
                    AllowAccessTokensViaBrowser = true,
                },

                new Client
                {
                    ClientId = "OrderService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"OrderService"},
                    ClientSecrets = {new Secret(identityConfigKeys.OrderServiceKey.Sha256())},
                    AllowAccessTokensViaBrowser = true,
                },

                new Client
                {
                    ClientId = "RestaurantService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"RestaurantService"},
                    ClientSecrets = {new Secret(identityConfigKeys.RestaurantServiceKey.Sha256())},
                    AllowAccessTokensViaBrowser = true,
                },
            };
        }

        public static IEnumerable<ApiResource> ApiResources(IConfiguration configuration)
        {
            var identityConfigKeys = Helpers.GetIdentityConfigKeys(configuration);


            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "FeedbackService",
                    Description = "FeedbackService resource",
                    ApiSecrets = {new Secret(identityConfigKeys.FeedbackServiceKey.Sha256())},
                    Scopes = {"FeedbackService",}
                },

                new ApiResource
                {
                    Name = "Gateway",
                    Description = "Gateway resource",
                    ApiSecrets = {new Secret(identityConfigKeys.GatewayKey.Sha256())},
                    Scopes = {"Gateway",}
                },

                new ApiResource
                {
                    Name = "DeliveryService",
                    Description = "DeliveryService resource",
                    ApiSecrets = {new Secret(identityConfigKeys.DeliveryServiceKey.Sha256())},
                    Scopes = {"DeliveryService",}
                },
                new ApiResource
                {
                    Name = "OrderService",
                    Description = "OrderService resource",
                    ApiSecrets = {new Secret(identityConfigKeys.OrderServiceKey.Sha256())},
                    Scopes = { "OrderService", }
                },

                new ApiResource
                {
                    Name = "RestaurantService",
                    Description = "RestaurantService resource",
                    ApiSecrets = {new Secret(identityConfigKeys.RestaurantServiceKey.Sha256())},
                    Scopes = { "RestaurantService", }
                },

            };
        }

        #endregion
    }
}