using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace UserService.Services
{
    public class UserUpdateCreditConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "user_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public UserUpdateCreditConsumer(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.CheckUserBalanceEvent);
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
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IUserService>();
                            var createOrderDto = JsonConvert.DeserializeObject<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                if (await myScopedService.CheckIfUserBalanceHasEnoughCreditForOrder(createOrderDto))
                                {
                                    if (await myScopedService.UpdateUserBalanceForOrder(createOrderDto))
                                    {
                                        var kafkaProducer = scope.ServiceProvider.GetRequiredService<IUserProducer>();

                                        await kafkaProducer.ProduceToKafka(
                                            EventStreamerEvents.CheckRestaurantStockEvent,
                                            jsonObj);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    stoppingToken.ThrowIfCancellationRequested();
                    consumerBuilder.Close();
                }
            }
        }
    }
}