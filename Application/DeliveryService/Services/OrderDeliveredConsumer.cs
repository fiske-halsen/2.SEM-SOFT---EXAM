using Common.Dto;
using Common.KafkaEvents;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace DeliveryService.Services
{
    public class OrderDeliveredConsumer : BackgroundService
    {

        #region private class properties

        private readonly string groupId = "delivery_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion

        #region DI services

        private readonly IServiceProvider _serviceProvider;

        #endregion

        public OrderDeliveredConsumer(IServiceProvider serviceProvider)
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
                consumerBuilder.Subscribe(EventStreamerEvents.OrderDeliveredEvent);
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
                            var orderDeliveredDto = JsonConvert.DeserializeObject<OrderDeliveredDto>(jsonObj);

                            if (orderDeliveredDto != null)
                            {
                                if (await deliveryService.UpdateDeliveryAsDelivered(orderDeliveredDto.DeliveryId))
                                {
                                    var kafkaProducer = scope.ServiceProvider.GetRequiredService<IDeliveryProducer>();

                                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.OrderInActiveEvent,
                                        jsonObj);
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
