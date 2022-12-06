using Common.Dto;

namespace GraphqlDemo.Services;

public class HelperService
{
    #region enums

    public enum ClientType
    {
        UserService,
        DeliveryService,
        ReviewService,
        OrderService,
        RestaurantService
    }

    #endregion

    private readonly IConfiguration _configuration;

    public HelperService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationCredentials GetMicroServiceApplicationCredentials(ClientType clientType)
    {
        string clientId = null;
        string clientSecret = null;
        string scope = null;

        switch (clientType)
        {
            case ClientType.DeliveryService:
                clientId = _configuration["DeliveryService:Host"];
                clientSecret = _configuration["DeliveryService:Key"];
                scope = _configuration["DeliveryService:Scope"];
                break;

            case ClientType.RestaurantService:
                clientId = _configuration["RestaurantService:Host"];
                clientSecret = _configuration["RestaurantService:Key"];
                scope = _configuration["RestaurantService:Scope"];
                break;

            case ClientType.OrderService:
                clientId = _configuration["OrderService:Host"];
                clientSecret = _configuration["OrderService:Key"];
                scope = _configuration["OrderService:Scope"];
                break;

            case ClientType.ReviewService:
                clientId = _configuration["FeedbackService:Host"];
                clientSecret = _configuration["FeedbackService:Key"];
                scope = _configuration["FeedbackService:Scope"];
                break;
            case ClientType.UserService:
                clientId = _configuration["IdentityServer:Host"];
                clientSecret = _configuration["IdentityServer:Key"];
                scope = _configuration["IdentityServer:Scope"];
                break;
            default:
                break;
        }

        return new ApplicationCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
        };
    }
}