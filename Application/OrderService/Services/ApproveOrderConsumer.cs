using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace OrderService.Services
{
    public class ApproveOrderConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "restaurant_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public ApproveOrderConsumer(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.ApproveOrderEvent);
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
                            var approveOrderDto = JsonConvert.DeserializeObject<ApproveOrderDto>(jsonObj); 

                            if (approveOrderDto != null)
                            {
                                if (await myScopedService.AcceptOrder(approveOrderDto.OrderId))
                                {
                                    var kafkaProducer = scope.ServiceProvider.GetRequiredService<IOrderProducer>();

                                    var emailObj = new EmailPackageDto
                                    {
                                        Email = approveOrderDto.CustomerEmail,
                                        Subject = "Order approved",
                                        Message = $"Order approved by restaurant approval"
                                    };

                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.NotifyUserEvent,
                                        JsonConvert.SerializeObject(emailObj));
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
