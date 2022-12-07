using Common.KafkaEvents;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using OrderService.Context;
using OrderService.ErrorHandling;
using OrderService.Repository;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

using (var adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = "localhost:9092"}).Build())
{
    try
    {
        //await adminClient.DeleteTopicsAsync(new List<string>()
        //    {EventStreamerEvents.StockValidEvent, EventStreamerEvents.CheckUserBalanceEvent});
        await adminClient.CreateTopicsAsync(new TopicSpecification[]
        {
            new TopicSpecification
                {Name = EventStreamerEvents.SaveOrderEvent, ReplicationFactor = 1, NumPartitions = 3},
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

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderService, OrderService.Services.OrdersService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

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