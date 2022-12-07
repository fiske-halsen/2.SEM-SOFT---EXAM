using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;

namespace PaymentValidatorService.Services
{
    public class PaymentValidatorConsumer : BackgroundService
    {
        private readonly string groupId = "create_order_group";
        private readonly string bootstrapServers = "localhost:9092";

        private readonly IServiceProvider _serviceProvider;

        public PaymentValidatorConsumer(IServiceProvider serviceProvider)
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
                            var obj = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(jsonObj);

                            if (obj != null)
                            {
                                if (await paymentValidatorService.ValidatePayment(obj))
                                {
                                    var kafkaProducer = scope.ServiceProvider
                                        .GetRequiredService<IPaymentValidatorProducer>();
                                    // Produce new event to kafka in case of valid payment types
                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.ValidPaymentEvent,
                                        jsonObj);
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