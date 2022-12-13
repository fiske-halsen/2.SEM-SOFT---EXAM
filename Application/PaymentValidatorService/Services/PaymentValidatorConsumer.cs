using Common.Dto;
using Common.HttpUtils;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace PaymentValidatorService.Services
{
    public class PaymentValidatorConsumer : BackgroundService
    {
        private readonly string groupId = "create_order_group";
        private readonly string bootstrapServers = "localhost:9092";

        private readonly IServiceProvider _serviceProvider;
        private readonly ISignalRWebSocketClient _signalRWebSocketClient;

        public PaymentValidatorConsumer(IServiceProvider serviceProvider)
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
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            using (var consumerBuilder = new ConsumerBuilder
                       <Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(EventStreamerEvents.ValidatePayment);
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
                            var paymentValidatorService =
                                scope.ServiceProvider.GetRequiredService<IPaymentValidatorService>();
                            var createOrderDto = JsonConvert.DeserializeObject<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                if (await paymentValidatorService.ValidatePayment(createOrderDto))
                                {
                                    var kafkaProducer = scope.ServiceProvider
                                        .GetRequiredService<IPaymentValidatorProducer>();
                                    // Produce new event to kafka in case of valid payment types
                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.ValidPaymentEvent,
                                        jsonObj);
                                }
                                else
                                {
                                    if (!_signalRWebSocketClient.IsConnected)
                                    {
                                        await _signalRWebSocketClient.Connect();
                                    }

                                    if (_signalRWebSocketClient.IsConnected)
                                    {
                                        // Notify the hub that it is not a valid payment
                                        await _signalRWebSocketClient.SendGenericResponse(new GenericResponse
                                        {
                                            Message = "Not a valid payment",
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
                    _signalRWebSocketClient.Dispose();
                    consumerBuilder.Close();
                }
            }
        }
    }
}