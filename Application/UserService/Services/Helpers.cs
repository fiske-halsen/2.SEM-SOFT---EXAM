using Common.Models;

namespace UserService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class Helpers
    {
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
    }
}