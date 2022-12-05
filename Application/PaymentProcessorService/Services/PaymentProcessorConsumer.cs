using Common.Dto;
using Confluent.Kafka;

namespace PaymentProcessorService.Services
{
    public class PaymentProcessorConsumer : BackgroundService
    {
        #region private class properties

        private readonly string topic = "valid_payment";
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
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest // Important to understand this part here; case if this client crashes
            };

            using (var consumerBuilder = new ConsumerBuilder
                       <Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = consumerBuilder.Consume
                            (cancelToken.Token);
                        var obj = consumer.Message.Value;


                        using (var scope = _serviceProvider.CreateScope())
                        {
                             var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                             var createOrderDto = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(obj);

                             if (createOrderDto != null)
                             {
                                 await paymentService.SimulatePayment(createOrderDto);
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
