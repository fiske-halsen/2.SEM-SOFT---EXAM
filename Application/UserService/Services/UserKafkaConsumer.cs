using Confluent.Kafka;
using System.Diagnostics;

namespace UserService.Services
{
    public class UserKafkaConsumer : BackgroundService
    {
        #region private class properties

        private readonly string topic = "payment_usercredit";
        private readonly string groupId = "user_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion
        #region DI services
        private readonly IServiceProvider _serviceProvider;

        #endregion

        public UserKafkaConsumer(IServiceProvider serviceProvider)
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
                        var test = consumer.Message.Value;


                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var myScopedService = scope.ServiceProvider.GetRequiredService<IUserService>();
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
            }


        }
    }
}