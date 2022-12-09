using Common.Dto;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IReviewServiceCommunicator
    {
        public Task<bool> CreateReview(CreateReviewDto createReviewDTO);
        public Task<IEnumerable<CreateReviewDto>> GetReviewsByUserId(int userId);
        public Task<IEnumerable<CreateReviewDto>> GetReviewsByRestaurantId(int restaurantId);
        public Task<IEnumerable<CreateReviewDto>> GetReviewsByDeliveryUserId(int deliveryUserId);
    }

    public class ReviewServiceCommunicator : IReviewServiceCommunicator
    {
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        private string _reviewServiceUrl;
        private readonly HelperService _helperService;
        private readonly ApplicationCredentials _applicationCredentials;

        public ReviewServiceCommunicator(IApiService apiService, IConfiguration configuration, HelperService helperService)
        {
            _apiService = apiService;
            _configuration = configuration;
            _reviewServiceUrl = configuration["FeedbackService:Host"];
            _helperService = helperService;
            _applicationCredentials = _helperService.GetMicroServiceApplicationCredentials(HelperService.ClientType.ReviewService);
        }

        /// <summary>
        /// Creates a review 
        /// </summary>
        /// <param name="createReviewDTO"></param>
        /// <returns></returns>
        public async Task<bool> CreateReview(CreateReviewDto createReviewDTO)
        {
            var reviewSerialized = JsonConvert.SerializeObject(createReviewDTO);
            return await _apiService.Post($"{_reviewServiceUrl}/review", reviewSerialized, _applicationCredentials);
        }

        /// <summary>
        /// Gets all the reviews made on a delivery user
        /// </summary>
        /// <param name="deliveryUserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByDeliveryUserId(int deliveryUserId)
        {
            return await _apiService.Get<CreateReviewDto>($"{_reviewServiceUrl}/user-reviews/{deliveryUserId}", _applicationCredentials);
        }

        /// <summary>
        /// Gets all the reviews made on a restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _apiService.Get<CreateReviewDto>($"{_reviewServiceUrl}/restaurant-reviews/{restaurantId}", _applicationCredentials);
        }

        /// <summary>
        /// Gets all the reviews made by a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByUserId(int userId)
        {
            return await _apiService.Get<CreateReviewDto>($"{_reviewServiceUrl}/delivery-driver-reviews/{userId}", _applicationCredentials);
        }
    }
}
