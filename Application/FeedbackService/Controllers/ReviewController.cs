using Common.Dto;
using FeedbackService.Models;
using FeedbackService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/review")]
        public async Task<bool> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            return await _reviewService.CreateReview(createReviewDto);
        }

        [Authorize]
        [HttpGet("/user-reviews/{userId}/")]
        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            return await _reviewService.GetReviewsByUserId(userId);
        }

        [Authorize]
        [HttpGet("/restaurant-reviews/{restaurantId}/")]
        public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _reviewService.GetReviewsByRestaurantId(restaurantId);
        }

        [Authorize]
        [HttpGet("/delivery-driver-reviews/{deliveryDriverId}/")]
        public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryDriverId)
        {
            return await _reviewService.GetReviewsByDeliveryUserId(deliveryDriverId);
        }
    }
}