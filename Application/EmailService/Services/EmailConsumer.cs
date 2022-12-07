using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;

namespace EmailService.Services
{
    public class EmailConsumer : BackgroundService
    {
        #region private class properties

        private readonly string groupId = "email_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public EmailConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
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
                consumerBuilder.Subscribe(EventStreamerEvents.NotifyUserEvent);
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
                            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                            var emailPackageDto = System.Text.Json.JsonSerializer.Deserialize<EmailPackageDto>(jsonObj);

                            if (emailPackageDto != null)
                            {
                                await emailService.SendEmail(emailPackageDto);
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