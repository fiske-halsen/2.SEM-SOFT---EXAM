using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace PaymentProcessorService.Services
{
    public class PaymentProcessorConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "user_valid_payments";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public PaymentProcessorConsumer(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.ValidPaymentEvent);
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
                            var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                            var createOrderDto = JsonConvert.DeserializeObject<CreateOrderDto>(jsonObj);

                            if (createOrderDto != null)
                            {
                                var kafkaProducer = scope.ServiceProvider
                                    .GetRequiredService<IKafkaPaymentProcessorProducer>();

                                await paymentService.SimulatePayment(createOrderDto, kafkaProducer);
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