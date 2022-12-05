using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
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
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset =
                    AutoOffsetReset.Earliest // Important to understand this part here; case if this client crashes
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
                        var msg_value = consumer.Message.Value;


                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var restaurantService = scope.ServiceProvider.GetRequiredService<IRestaurantService>();

                            var createOrderDto = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(msg_value);

                            if (createOrderDto != null)
                            {
                                await restaurantService.CheckMenuItemStock(createOrderDto);
                                await restaurantService.UpdateMenuItemStock(createOrderDto);
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