using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Confluent.Kafka;
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
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        #endregion

        public SaveOrderConsumer(IServiceProvider serviceProvider)
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
                            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                            var createOrderDto = JsonConvert.DeserializeObject<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                GenericResponse genericResponse = new GenericResponse
                                    {Message = "Could not create order", Status = "404"};

                                if (!_signalRWebSocketClient.IsConnected)
                                {
                                    await _signalRWebSocketClient.Connect();
                                }

                                var isOrderCreated = await orderService.CreateOrder(createOrderDto);

                                if (isOrderCreated)
                                {
                                    var kafkaProducer = scope.ServiceProvider.GetRequiredService<IOrderProducer>();

                                    var emailObj = new EmailPackageDto
                                    {
                                        Email = createOrderDto.CustomerEmail,
                                        Subject = "Order received",
                                        Message = $"Order received waiting for restaurant approval"
                                    };

                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.NotifyUserEvent,
                                        JsonConvert.SerializeObject(emailObj));


                                    genericResponse.Message = "Order received waiting for restaurant approval";
                                    genericResponse.Status = "200";
                                }


                                if (_signalRWebSocketClient.IsConnected)
                                {
                                    // Notify the hub that it is not a valid payment
                                    await _signalRWebSocketClient.SendGenericResponse(genericResponse);

                                    if (isOrderCreated && createOrderDto != null)
                                    {
                                       await _signalRWebSocketClient.SendNewOrderToRestaurantOwner(createOrderDto);
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