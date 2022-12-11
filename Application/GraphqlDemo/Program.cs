using GraphqlDemo.Authorization;
using GraphqlDemo.ErrorHandling;
using GraphqlDemo.Operations;
using GraphqlDemo.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDeliveryServiceCommunicator, DeliveryServiceCommunicator>();
builder.Services.AddScoped<IRestaurantServiceCommunicator, RestaurantServiceCommunicator>();
builder.Services.AddScoped<IOrderServiceCommunicator, OrderServiceCommunicator>();
builder.Services.AddScoped<IReviewServiceCommunicator, ReviewServiceCommunicator>();
builder.Services.AddScoped<IUserServiceCommunicator, UserServiceCommunicator>();
builder.Services.AddScoped<HelperService>();
builder.Services.AddSingleton<IAuthorizationHandler, HasRoleTypeHandler>();

builder.Services.AddHttpContextAccessor();

var identityServer = configuration["IdentityServer:Host"];


builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

//// Authentication
builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = identityServer;
        options.TokenValidationParameters.ValidateAudience = true;
        options.Audience = "Gateway";
        options.TokenValidationParameters.ValidTypes = new[] {"at+jwt"};
        options.RequireHttpsMetadata = false;
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy => policy.AddRequirements(new RoleTypeRequirement(identityServer, 1)));
    options.AddPolicy("RestaurantOwner", policy => policy.AddRequirements(new RoleTypeRequirement(identityServer, 2)));
    options.AddPolicy("Delivery", policy => policy.AddRequirements(new RoleTypeRequirement(identityServer, 3)));
});

string AllowedOrigin = "allowedOrigin";

builder.Services.AddCors(option =>
{
    option.AddPolicy(AllowedOrigin, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
builder.WebHost.UseUrls("https://localhost:5009");

app.UseRouting();
app.ConfigureExceptionHandler();

app.UseCors(AllowedOrigin);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapGraphQL().RequireAuthorization(); });


app.Run();