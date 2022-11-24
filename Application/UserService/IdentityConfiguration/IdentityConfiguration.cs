using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace UserService.IdentityConfiguration
{
    public class IdentityConfiguration
    {
        #region security configuration
        /// <summary>
        /// All the api scopes for the different services
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("Gateway", "GraphQL Gateway"),
            new ApiScope("DeliveryService", "Delivery Service"),
            new ApiScope("EmailService", "Email Service"),
            new ApiScope("FeedbackService", "Feedback Service"),
            new ApiScope("OrderService", "Order Service"),
            new ApiScope("RestaurantService", "Restaurant Service"),
        };

        /// <summary>
        /// All the clients (services) that needs a key to be communicated with
        /// </summary>
        /// <param name="configuration"> the </param>
        /// <returns></returns>
        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var gatewaySecret = configuration["Gateway:Key"];
            var deliverySecret = configuration["DeliveryService:Key"];
            var emailSecret = configuration["EmailService:Key"];
            var feedbackSecret = configuration["FeedbackService:Key"];
            var orderSecret = configuration["OrderService:Key"];
            var restaurantSecret = configuration["RestaurantService:Key"];

            // Get the configuration for secrets
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Gateway",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "Gateway" },
                    ClientSecrets = { new Secret(gatewaySecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

                new Client
                {
                    ClientId = "DeliveryService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "DeliveryService" },
                    ClientSecrets = { new Secret(deliverySecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

                new Client
                {
                    ClientId = "EmailService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "EmailService" },
                    ClientSecrets = { new Secret(emailSecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

                new Client
                {
                    ClientId = "FeedbackService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "FeedbackService" },
                    ClientSecrets = { new Secret(feedbackSecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

                new Client
                {
                    ClientId = "OrderService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "OrderService" },
                    ClientSecrets = { new Secret(orderSecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

                new Client
                {
                    ClientId = "RestaurantService",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "RestaurantService" },
                    ClientSecrets = { new Secret(restaurantSecret.Sha256())},
                    AllowAccessTokensViaBrowser = true
                },

            };

            public static IEnumerable<ApiResource> ApiResources(IConfiguration configuration)
            {
                var gatewaySecret = configuration["Gateway:Key"];
                var deliverySecret = configuration["DeliveryService:Key"];
                var emailSecret = configuration["EmailService:Key"];
                var feedbackSecret = configuration["FeedbackService:Key"];
                var orderSecret = configuration["OrderService:Key"];
                var restaurantSecret = configuration["RestaurantService:Key"]




            };

        }

        #endregion
    }
}
