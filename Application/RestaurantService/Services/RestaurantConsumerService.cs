using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;
using RestaurantService.Model;

namespace RestaurantService.Services
{
    public class RestaurantConsumerService : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "restaurant_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public RestaurantConsumerService(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.CheckRestaurantStockEvent);
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
                            var restaurantService = scope.ServiceProvider.GetRequiredService<IRestaurantService>();

                            var createOrderDto = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                if (await restaurantService.CheckMenuItemStock(createOrderDto))
                                {
                                    if (await restaurantService.UpdateMenuItemStock(createOrderDto))
                                    {
                                        var kafkaProducer = scope.ServiceProvider
                                            .GetRequiredService<IRestaurantProducerService>();

                                        // Notify our order service..
                                        await kafkaProducer.ProduceToKafka(EventStreamerEvents.SaveOrderEvent,
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