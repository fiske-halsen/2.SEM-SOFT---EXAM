using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace RestaurantService.Services
{
    public class RestaurantUpdateStockConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "restaurant_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        public RestaurantUpdateStockConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _signalRWebSocketClient = new SignalRWebSocketClient();
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
                consumerBuilder.Subscribe(EventStreamerEvents.UpdateRestaurantStockEvent);
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

                            var approveOrderDto = JsonConvert.DeserializeObject<ApproveOrderDto>(jsonObj);

                            if (approveOrderDto != null)
                            {
                                if (await restaurantService.UpdateMenuItemStock(approveOrderDto))
                                {
                                    if (!_signalRWebSocketClient.IsConnected)
                                    {
                                        await _signalRWebSocketClient.Connect();
                                    }

                                    if (_signalRWebSocketClient.IsConnected)
                                    {
                                        // Notify the hub that it the stock is updated..
                                        await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                        {
                                            Message = "Menu item stock updated",
                                            Status = "200"
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

        #endregion
    }
}