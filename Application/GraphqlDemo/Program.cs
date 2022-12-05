using GraphqlDemo.ErrorHandling;
using GraphqlDemo.Operations;
using GraphqlDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDeliveryServiceCommunicator, DeliveryServiceCommunicator>();
builder.Services.AddScoped<IRestaurantServiceCommunicator, RestaurantServiceCommunicator>();
builder.Services.AddScoped<IOrderServiceCommunicator, OrderServiceCommunicator>();
builder.Services.AddScoped<IReviewServiceCommunicator, ReviewServiceCommunicator>();
builder.Services.AddScoped<IUserServiceCommunicator, UserServiceCommunicator>();
builder.Services.AddScoped<HelperService>();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();


var app = builder.Build();

app.ConfigureExceptionHandler();

app.MapGraphQL();

app.Run();