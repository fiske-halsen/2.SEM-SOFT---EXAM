using System.Diagnostics;
using Common.Dto;
using Common.KafkaEvents;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IOrderServiceCommunicator
    {
        public Task<IEnumerable<OrderDto>> GetOrdersForRestaurants(int restaurantId);
        public Task<IEnumerable<OrderDto>> GetOrdersForRestaurants(int restaurantId, bool isApproved);
        public Task<IEnumerable<OrderDto>> GetAllOrdersForRestaurantsByUser(string userEmail);
        public Task<bool> CreateOrder(CreateOrderDto createOrderDto);
        public Task<bool> ApproveOrder(ApproveOrderDto approveOrderDto);
    }

    public class OrderServiceCommunicator : IOrderServiceCommunicator
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HelperService _helperService;
        private readonly ApplicationCredentials _applicationCredentials;
        private readonly IKafkaProducerService _kafkaProducerService;
        private string _orderServiceUrl;

        public OrderServiceCommunicator(
            IApiService apiService,
            ITokenService tokenService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            HelperService helperService,
            IKafkaProducerService kafkaProducerService)

        {
            _apiService = apiService;
            _tokenService = tokenService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _helperService = helperService;
            _orderServiceUrl = configuration["OrderService:Host"];
            _applicationCredentials =
                _helperService.GetMicroServiceApplicationCredentials(HelperService.ClientType.OrderService);
            _kafkaProducerService = kafkaProducerService;
        }

        /// <summary>
        /// Approves a order by publishing a event to kafka
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

        /// <summary>
        /// Creates a order by posting it to kafka
        /// </summary>
        /// <param name="createOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var orderSerialized = JsonConvert.SerializeObject(createOrderDto);
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
        /// Gets all orders for restaurant by a given user email and restaurant id
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<OrderDto>> GetAllOrdersForRestaurantsByUser( string userEmail)
        {
            return await _apiService.Get<OrderDto>($"{_orderServiceUrl}/users/{userEmail}",
                _applicationCredentials);
        }

        /// <summary>
        /// Gets all orders for a given restaurant by restaurant id
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<OrderDto>> GetOrdersForRestaurants(int restaurantId)
        {
            return await _apiService.Get<OrderDto>($"{_orderServiceUrl}/restaurants/{restaurantId}",
                _applicationCredentials);
        }

        /// <summary>
        /// Gets all approved orders for a given restaurant 
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<OrderDto>> GetOrdersForRestaurants(int restaurantId, bool isApproved)
        {
            return await _apiService.Get<OrderDto>($"{_orderServiceUrl}/restaurants/{restaurantId}/{isApproved}",
                _applicationCredentials);
        }
    }
}