using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace RestaurantService.Services
{
    public class RestaurantConsumerStockCheck : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "restaurant_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        #endregion

        public RestaurantConsumerStockCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            using (var scope = _serviceProvider.CreateScope())
            {
                var signalRWebSocketClient = scope.ServiceProvider.GetRequiredService<ISignalRWebSocketClient>();
            }
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

                            var createOrderDto = JsonConvert.DeserializeObject<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                if (!_signalRWebSocketClient.IsConnected)
                                {
                                    await _signalRWebSocketClient.Connect();
                                }

                                try
                                {
                                    if (await restaurantService.CheckMenuItemStock(createOrderDto))
                                    {

                                        var kafkaProducer = scope.ServiceProvider
                                            .GetRequiredService<IRestaurantProducerService>();

                                        // Notify our order service..
                                        await kafkaProducer.ProduceToKafka(EventStreamerEvents.SaveOrderEvent,
                                            jsonObj);
                                    }
                                }
                                catch (Exception e)
                                {

                                    if (_signalRWebSocketClient.IsConnected)
                                    {
                                        // Notify the hub that it is not a valid payment
                                        await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                        {
                                            Message = e.Message,
                                            Status = "404"
                                        });
                                    }
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