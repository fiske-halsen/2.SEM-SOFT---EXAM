using Common.ErrorModels;
using FeedbackService.DTO;
using FeedbackService.Models;
using FeedbackService.Repository;

namespace FeedbackService.Services
{

    public interface IReviewService
    {

        public Task<bool> CreateReview(CreateReviewDTO createReviewDTO);
        public Task<List<Review>> GetReviewsByUserId(int userId);
        public Task<List<Review>> GetReviewsByRestaurantId(int restaurantId);
        public Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId);
    }

    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> CreateReview(CreateReviewDTO createReviewDTO)
        {
            if (createReviewDTO.Rating > 5)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Rating cant be higher than 5");

            }
            var review = new Review
            {
                UserId = createReviewDTO.UserId,
                RestaurantId = createReviewDTO.RestaurantId,
                DeliveryDriverId = createReviewDTO.DeliveryDriverId,
                ReviewText = createReviewDTO.ReviewText,
                ReviewDate = createReviewDTO.ReviewDate,
                OrderId = createReviewDTO.OrderId,
                Rating = createReviewDTO.Rating
            };
            await _reviewRepository.CreateReview(review);
            return true;

        }

        public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId)
        {

            var reviews = await _reviewRepository.GetReviewsByDeliveryUserId(deliveryUserId);
            if (!reviews.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given id not found");
            }
            return reviews;
        }




        public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
        {

            var reviews = await _reviewRepository.GetReviewsByRestaurantId(restaurantId);
            if (!reviews.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given id not found");
            }
            return reviews;
        }

        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserId(userId);
            if (!reviews.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given id not found");
            }
            return reviews;
        }

    }
}
