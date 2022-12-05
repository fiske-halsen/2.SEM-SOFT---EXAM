using System.Diagnostics;
using Common.Dto;
using Common.KafkaEvents;
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

        public async Task<bool> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                CreateOrderDto orderDto = new CreateOrderDto
                {
                    PaymentType = dto.PaymentType,
                    CustomerEmail = dto.CustomerEmail,
                    RestaurantId = dto.RestaurantId,
                    OrderTotal = dto.OrderTotal,
                    MenuItems = dto.MenuItems
                };

                var orderSerialized = JsonConvert.SerializeObject(orderDto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.CreateOrderEvent, orderSerialized);

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
