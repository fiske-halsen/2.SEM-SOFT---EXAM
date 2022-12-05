using Common.Dto;
using Confluent.Kafka;
using RestaurantService.Model;

namespace RestaurantService.Services
{
    public class Consumer: BackgroundService
    {
        #region private class properties

        private readonly string topic = "check_restaurant_stock";
        private readonly string groupId = "restaurant_group";
        private readonly string bootstrapServers = "localhost:9092";

        #endregion
        #region DI services
        private readonly IServiceProvider _serviceProvider;

        #endregion

        public Consumer(IServiceProvider serviceProvider)
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
                        var msg_value = consumer.Message.Value;

                        
                        using (var scope = _serviceProvider.CreateScope())
                        {
                             var myScopedService = scope.ServiceProvider.GetRequiredService<IRestaurantService>();
                             
                            var CreateOrderDto = System.Text.Json.JsonSerializer.Deserialize<CreateOrderDto>(msg_value);
                            await myScopedService.CheckMenuItemStock(CreateOrderDto);
                            await myScopedService.UpdateMenuItemStock(CreateOrderDto);

                            //publish event 

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
