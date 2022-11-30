using Common.Models;
using GraphqlDemo.Services;
using Newtonsoft.Json;

namespace GraphqlDemo.Operations
{
    public class Mutation
    {
        private readonly KafkaProducerService _kafkaProducerService;

        public Mutation()
        {
            _kafkaProducerService = new KafkaProducerService();
        }

        public async Task<Order> CreateOrder(List<MenuItem> menuItems, string customer, string restaurant, float total)
        {
            Order order = new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Restaurant = restaurant,
                Total = total,
                Items = menuItems
            };

            var orderSerialized = JsonConvert.SerializeObject(order);

            await _kafkaProducerService.SendReviewRequest("create_order", orderSerialized);

            return order;
        }
    }
}
