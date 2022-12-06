using Common.KafkaEvents;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Context;
using RestaurantService.ErrorHandling;
using RestaurantService.Repository;
using RestaurantService.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = "localhost:9092" }).Build())
{
    try
    {
        //await adminClient.DeleteTopicsAsync(new List<string>() { EventStreamerEvents.CheckRestaurantStockEvent });

        await adminClient.CreateTopicsAsync(new TopicSpecification[] {
            new TopicSpecification { Name = EventStreamerEvents.CheckRestaurantStockEvent, ReplicationFactor = 1, NumPartitions = 3 } });
    }
    catch (CreateTopicsException e)
    {
        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    }
}


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService.Services.RestaurantService>();
builder.Services.AddScoped<IRestaurantProducerService, RestaurantProducerService>();
builder.Services.AddHostedService<RestaurantConsumerService>();

builder.Services.AddDbContext<DBApplicationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();