using FeedbackService.DTO;
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

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> CreateReview([FromBody] CreateReviewDTO createReviewDto)
        {
            return await _reviewService.CreateReview(createReviewDto);
        }

        [HttpGet("/{userId}/userreviews")]
        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            return await _reviewService.GetReviewsByUserId(userId);
        }

        [HttpGet("/{restaurantId}/restaurantreviews")]
        public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _reviewService.GetReviewsByRestaurantId(restaurantId);
        }

        [HttpGet("/{deliveryDriverId}/deliverydriverreviews")]
        public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryDriverId)
        {
            return await _reviewService.GetReviewsByDeliveryUserId(deliveryDriverId);
        }
    }
}