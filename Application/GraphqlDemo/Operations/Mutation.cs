using Common.Dto;
using Common.KafkaEvents;
using GraphqlDemo.Services;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GraphqlDemo.Operations
{
    public class Mutation
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IConfiguration _configuration;
        private readonly IUserServiceCommunicator _userServiceCommunicator;

        public Mutation(
            IKafkaProducerService kafkaProducerService,
            IConfiguration configuration,
            IUserServiceCommunicator userServiceCommunicator)
        {
            _kafkaProducerService = kafkaProducerService;
            _configuration = configuration;
            _userServiceCommunicator = userServiceCommunicator;
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
                    MenuItems = dto.MenuItems,
                    CardType = dto.CardType,
                    VoucherCode = dto.VoucherCode
                };

                var orderSerialized = JsonConvert.SerializeObject(orderDto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.ValidatePayment, orderSerialized);

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
        /// <param Name="createUserDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                await _userServiceCommunicator.CreateUser(createUserDto);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Login for a user
        /// </summary>
        /// <param Name="loginUserDto"></param>
        /// <returns></returns>
        public async Task<TokenDto> Login(LoginUserDto loginUserDto)
        {
            try
            {
                return await _userServiceCommunicator.Login(loginUserDto);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}