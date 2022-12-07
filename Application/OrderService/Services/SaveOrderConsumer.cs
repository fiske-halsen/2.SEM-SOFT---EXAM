using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OrderService.Services
{
    public class SaveOrderConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "user_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public SaveOrderConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset =
                    AutoOffsetReset.Earliest, // Important to understand this part here; case if this client crashes
                AllowAutoCreateTopics = true
            };

            using (var consumerBuilder = new ConsumerBuilder
                       <Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(EventStreamerEvents.SaveOrderEvent);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = consumerBuilder.Consume
                            (cancelToken.Token);
                        var jsonObj = consumer.Message.Value;

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                            var createOrderDto = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                if (await myScopedService.CreateOrder(createOrderDto))
                                {
                                    var kafkaProducer = scope.ServiceProvider.GetRequiredService<IOrderProducer>();

                                    var emailObj = new EmailPackageDto
                                    {
                                        Email = createOrderDto.CustomerEmail,
                                        Subject = "Order received",
                                        Message = $"Order received waiting for restaurant approval"
                                    };

                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.NotifyUserEvent, JsonConvert.SerializeObject(emailObj));

                                }
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }


    }
}
