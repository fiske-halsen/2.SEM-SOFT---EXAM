using Confluent.Kafka;
using System.Diagnostics;
using System.Net;

namespace DeliveryService.Services
{
    public interface IDeliveryProducer
    {
        public Task<bool> ProduceToKafka(string topic, string data);
    }

    public class DeliveryProducer : IDeliveryProducer
    {
        private readonly string server = "localhost:9092";

        public async Task<bool> ProduceToKafka(string topic, string data)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = server,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string>
                    {
                        Value = data
                    });

                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return await Task.FromResult(false);
        }
    }
}