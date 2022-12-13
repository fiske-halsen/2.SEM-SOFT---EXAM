using Common.HttpUtils;
using Common.KafkaEvents;
using Common.KafkaProducer;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using DeliveryService.Context;
using DeliveryService.ErrorHandling;
using DeliveryService.Repository;
using DeliveryService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

using (var adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = "localhost:9092"}).Build())
{
    try
    {
        await adminClient.CreateTopicsAsync(new TopicSpecification[]

        {
            new TopicSpecification
                {Name = EventStreamerEvents.CreateDeliveryEvent, ReplicationFactor = 1, NumPartitions = 3},
            new TopicSpecification
                {Name = EventStreamerEvents.OrderDeliveredEvent, ReplicationFactor = 1, NumPartitions = 3},
        });
    }
    catch (CreateTopicsException e)
    {
        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    }
}


var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DbApplicationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliverySerivice, DeliveryService.Services.DeliveryService>();
builder.Services.AddScoped<IGenericKafkaProducer, GenericKafkaProducer>();
builder.Services.AddHostedService<CreateDeliveryConsumer>();
builder.Services.AddHostedService<OrderDeliveredConsumer>();
builder.Services.AddScoped<ISignalRWebSocketClient, SignalRWebSocketClient>();

var identityServer = configuration["IdentityServer:Host"];

//// Authentication
builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = identityServer;
        options.TokenValidationParameters.ValidateAudience = true;
        options.Audience = "DeliveryService";
        options.TokenValidationParameters.ValidTypes = new[] {"at+jwt"};
        options.RequireHttpsMetadata = false;
    });


builder.Services.AddAuthorization();

var app = builder.Build();
builder.WebHost.UseUrls("https://localhost:5005");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbApplicationContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
        db.Database.EnsureCreated();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
    .AllowCredentials());


app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// For integration testing purposes; Woops! Needed because program is behind the scenes a internal class, we need a public way to get it
public partial class Program
{
}