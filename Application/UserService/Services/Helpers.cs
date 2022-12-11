using System.Security.Claims;
using Common.Enums;
using Common.Models;
using UserService.Models;

namespace UserService.Services
{
    /// <summary>
    /// Helpers regarding security measures
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Gets the identity key configs for each service
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IdentityKeyConfig GetIdentityConfigKeys(IConfiguration configuration)
        {
            return new IdentityKeyConfig
            {
                FeedbackServiceKey = configuration["FeedbackService:Key"],
                DeliveryServiceKey = configuration["DeliveryService:Key"],
                GatewayKey = configuration["Gateway:Key"],
                OrderServiceKey = configuration["OrderService:Key"],
                RestaurantServiceKey = configuration["RestaurantService:Key"],
                UserServiceKey = configuration["UserService:Key"]
            };
        }

        /// <summary>
        /// Gets the correct claim for a specific role; used for claim based authorization
        /// </summary>
        /// <param name="roleTypes"></param>
        /// <returns></returns>
        public static Claim GetRoleTypeClaim(RoleTypes roleTypes)
        {
            switch (roleTypes)
            {
                case RoleTypes.Customer:
                    return new Claim("Customer", "Customer");
                    break;
                case RoleTypes.DeliveryPerson:
                    return new Claim("Delivery", "Delivery");
                    break;

                case RoleTypes.RestaurantOwner:
                    return new Claim("RestaurantOwner", "RestaurantOwner");
                    break;
            }

            return null;
        }
    }
}