using GraphqlDemo.ErrorHandling;
using GraphqlDemo.Operations;
using GraphqlDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();


var app = builder.Build();


app.ConfigureExceptionHandler();

app.MapGraphQL();

app.Run();
