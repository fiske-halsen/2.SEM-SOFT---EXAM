using Common.KafkaEvents;
using Confluent.Kafka.Admin;
using Confluent.Kafka;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);
using (var adminClient = new AdminClientBuilder(new AdminClientConfig {BootstrapServers = "localhost:9092"}).Build())
{
    try
    {
        await adminClient.CreateTopicsAsync(new TopicSpecification[]
        {
            new TopicSpecification
                {Name = EventStreamerEvents.NotifyUserEvent, ReplicationFactor = 1, NumPartitions = 3},
        });
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

builder.Services.AddHostedService<EmailConsumer>();
builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();

var app = builder.Build();
builder.WebHost.UseUrls("https://localhost:5006");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();