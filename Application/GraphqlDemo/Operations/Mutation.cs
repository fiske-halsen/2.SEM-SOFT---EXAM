using System.Diagnostics;
using Common.Dto;
using Common.Models;
using GraphqlDemo.Services;
using Newtonsoft.Json;

namespace GraphqlDemo.Operations
{
    public class Mutation
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IConfiguration _configuration;

        public Mutation(IKafkaProducerService kafkaProducerService, IConfiguration configuration)
        {
            _kafkaProducerService = kafkaProducerService;
            _configuration = configuration;
        }

        public async Task<bool> CreateOrder(List<MenuItem> menuItems, string customer, string restaurant, float total)
        {
            try
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

                await _kafkaProducerService.ProduceToKafka("create_order", orderSerialized);

                return true;
            }
            catch (Exception e)
            {
                return false;

            }
        }

        /// <summary>
        /// Creates a new user to the system; Sends a call to UserService
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            try
            {



                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

        }
    }
}
