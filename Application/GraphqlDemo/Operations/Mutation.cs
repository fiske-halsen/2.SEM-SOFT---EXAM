using Common.Models;
using GraphqlDemo.Services;

namespace GraphqlDemo.Operations
{
    public class Mutation
    {
        private readonly List<Order> _order;
        private readonly KafkaProducerService _kafkaProducerService;

        public Mutation()
        {
            _order = new List<Order>();
            _kafkaProducerService = new KafkaProducerService();
        }

        public bool CreateOrder(List<MenuItem> menuItems, string customer, string restaurant, float total)
        {
            Order order = new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Restaurant = restaurant,
                Total = total,
                Items = menuItems
            };

            _order.Add(order);



            return true;
        }
    }
}
