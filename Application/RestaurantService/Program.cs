using Common.KafkaEvents;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Context;
using RestaurantService.ErrorHandling;
using RestaurantService.Repository;
using RestaurantService.Services;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Serilog.Core;
using System.Collections.ObjectModel;
using System.Data;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day).WriteTo.MSSqlServer(
        appConfiguration:appSettings,
        connectionString:appSettings.GetConnectionString("DefaultConnection"),
        tableName:"Logs"

        
        
        )

    .ReadFrom.Configuration(ctx.Configuration));


using (var adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = "localhost:9092"}).Build())
{
    try
    {
        await adminClient.CreateTopicsAsync(new TopicSpecification[]
        {
            new TopicSpecification
                {Name = EventStreamerEvents.UpdateRestaurantStockEvent, ReplicationFactor = 1, NumPartitions = 3}
        });

        await adminClient.CreateTopicsAsync(new TopicSpecification[]
        {
            new TopicSpecification
                {Name = EventStreamerEvents.CheckRestaurantStockEvent, ReplicationFactor = 1, NumPartitions = 3}
        });
    }
    catch (CreateTopicsException e)
    {
        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    }
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService.Services.RestaurantService>();
builder.Services.AddScoped<IRestaurantProducerService, RestaurantProducerService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<RestaurantConsumerStockCheck>();
builder.Services.AddHostedService<RestaurantUpdateStockConsumer>();

builder.Services.AddDbContext<DBApplicationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var identityServer = configuration["IdentityServer:Host"];

//// Authentication
builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = identityServer;
        options.TokenValidationParameters.ValidateAudience = true;
        options.Audience = "RestaurantService";
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        options.RequireHttpsMetadata = false;
    });


builder.Services.AddAuthorization();

var app = builder.Build();

//builder.WebHost.UseUrls("https://localhost:5002");

app.UseSerilogRequestLogging();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DBApplicationContext>();
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

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<IDbLogger>();
    app.ConfigureExceptionHandler(logger);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// For integration testing purposes; Woops! Needed because program is behind the scenes a internal class, we need a public way to get it
public partial class Program
{
}