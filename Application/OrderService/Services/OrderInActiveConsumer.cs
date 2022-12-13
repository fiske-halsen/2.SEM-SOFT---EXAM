using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Common.KafkaProducer;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace OrderService.Services
{
    public class OrderInActiveConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "user_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        #endregion

        public OrderInActiveConsumer(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.OrderInActiveEvent);
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
                            var orderDeliveredDto = JsonConvert.DeserializeObject<OrderDeliveredDto>(jsonObj);

                            if (orderDeliveredDto != null)
                            {
                                if (!_signalRWebSocketClient.IsConnected)
                                {
                                    await _signalRWebSocketClient.Connect();
                                }

                                try
                                {
                                    if (await orderService.UpdateOrderSetInActive(orderDeliveredDto.OrderId))
                                    {
                                        var kafkaProducer = scope.ServiceProvider
                                            .GetRequiredService<IGenericKafkaProducer>();

                                        var emailObj = new EmailPackageDto
                                        {
                                            Email = orderDeliveredDto.UserEmail,
                                            Subject = "Order received",
                                            Message = $"You recently had a order delivered; please provide us feedback!"
                                        };

                                        await kafkaProducer.ProduceToKafka(EventStreamerEvents.NotifyUserEvent,
                                            JsonConvert.SerializeObject(emailObj));

                                        if (_signalRWebSocketClient.IsConnected)
                                        {
                                            // Notify the hub that it the stock is updated..
                                            await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                            {
                                                Message =
                                                    "You recently had a order delivered; please provide us feedback!",
                                                Status = "200"
                                            });
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    if (_signalRWebSocketClient.IsConnected)
                                    {
                                        // Notify the hub that it the stock is updated..
                                        await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                        {
                                            Message = e.Message,
                                            Status = "500"
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