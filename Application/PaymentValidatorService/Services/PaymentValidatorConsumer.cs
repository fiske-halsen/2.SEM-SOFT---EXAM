using Confluent.Kafka;
using static PaymentValidatorService.Services.PaymentValidatorService;

namespace PaymentValidatorService.Services
{
    public class PaymentValidatorConsumer : BackgroundService
    {
        private readonly string topic = "create_order";
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
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = consumerBuilder.Consume
                           (cancelToken.Token);
                        var test = consumer.Message.Value;


                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IPaymentValidatorSerice>();
                            //var obj = System.Text.Json.JsonSerializer.Deserialize<ReviewDto>(test);
                            //Debug.WriteLine(obj.Message);

                            //await myScopedService.SaveReview(obj);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            };


        }
    }
}