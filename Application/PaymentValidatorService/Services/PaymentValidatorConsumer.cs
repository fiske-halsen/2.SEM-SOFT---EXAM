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

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumerBuilder = new ConsumerBuilder
                       <Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(EventStreamerEvents.CreateOrderEvent);
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
                                await paymentValidatorService.ValidatePayment(obj);
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