using System.Diagnostics;
using Common.Dto;
using Common.KafkaEvents;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IDeliveryServiceCommunicator
    {
        public Task<bool> CreateDelivery(CreateDeliveryDto createDeliveryDto);
        public Task<bool> UpdateDeliveryToDelivered(OrderDeliveredDto orderDeliveredDto);
    }

    public class DeliveryServiceCommunicator : IDeliveryServiceCommunicator
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HelperService _helperService;
        private readonly ApplicationCredentials _applicationCredentials;
        private readonly IKafkaProducerService _kafkaProducerService;
        private string _deliveryServiceUrl;

        public DeliveryServiceCommunicator(
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
            _deliveryServiceUrl = configuration["DeliveryService:Host"];
            _applicationCredentials =
                _helperService.GetMicroServiceApplicationCredentials(HelperService.ClientType.DeliveryService);
            _kafkaProducerService = kafkaProducerService;
        }

        /// <summary>
        /// Send a call to kafka posting a create delivery event
        /// </summary>
        /// <param name="createDeliveryDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateDelivery(CreateDeliveryDto createDeliveryDto)
        {
            try
            {
                var createDeliverySerialized = JsonConvert.SerializeObject(createDeliveryDto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.CreateDeliveryEvent,
                    createDeliverySerialized);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates the delivery to delivered by sending a event to Kafka
        /// </summary>
        /// <param name="orderDeliveredDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateDeliveryToDelivered(OrderDeliveredDto orderDeliveredDto)
        {
            try
            {
                var orderDeliveredDtoSerialized = JsonConvert.SerializeObject(orderDeliveredDto);
                await _kafkaProducerService.ProduceToKafka(EventStreamerEvents.OrderDeliveredEvent, orderDeliveredDtoSerialized);
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