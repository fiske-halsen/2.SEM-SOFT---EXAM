using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Common.KafkaProducer;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace DeliveryService.Services
{
    public class CreateDeliveryConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "delivery_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        #endregion

        public CreateDeliveryConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            using (var scope = _serviceProvider.CreateScope())
            {
                _signalRWebSocketClient = scope.ServiceProvider.GetRequiredService<ISignalRWebSocketClient>();
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
                consumerBuilder.Subscribe(EventStreamerEvents.CreateDeliveryEvent);
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
                            var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliverySerivice>();
                            var createDeliveryDto = JsonConvert.DeserializeObject<CreateDeliveryDto>(jsonObj);

                            try
                            {
                                if (!_signalRWebSocketClient.IsConnected)
                                {
                                    await _signalRWebSocketClient.Connect();
                                }

                                if (createDeliveryDto != null)
                                {
                                    if (await deliveryService.CreateDelivery(createDeliveryDto))
                                    {
                                        var kafkaProducer =
                                            scope.ServiceProvider.GetRequiredService<IGenericKafkaProducer>();

                                        var emailObj = new EmailPackageDto
                                        {
                                            Email = createDeliveryDto.UserEmail,
                                            Subject = "Order received",
                                            Message = $"Order accepted by delivery guy"
                                        };

                                        await kafkaProducer.ProduceToKafka(EventStreamerEvents.NotifyUserEvent,
                                            JsonConvert.SerializeObject(emailObj));

                                        if (_signalRWebSocketClient.IsConnected)
                                        {
                                            // Notify the hub that it is not a valid payment
                                            await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                            {
                                                Message = "Order accepted by delivery guy",
                                                Status = "200"
                                            });
                                        }
                                    }
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
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }
    }
}