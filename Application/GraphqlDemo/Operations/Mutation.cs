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

        #region OrderService

        /// <summary>
        /// Creates a new order and posts a ValidatePayment to kafka
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                var orderSerialized = JsonConvert.SerializeObject(dto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.ValidatePayment, orderSerialized);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Approves order and orchestrates events to both restaurant service and order service
        /// </summary>
        /// <param name="approveOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> ApproveOrder(ApproveOrderDto approveOrderDto)
        {
            try
            {
                var approveOrderSerialized = JsonConvert.SerializeObject(approveOrderDto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.ApproveOrderEvent,
                    approveOrderSerialized);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.UpdateRestaurantStockEvent,
                    approveOrderSerialized);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region UserService

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

        #endregion
    }
}