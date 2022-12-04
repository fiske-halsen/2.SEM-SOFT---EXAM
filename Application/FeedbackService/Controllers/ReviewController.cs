﻿using FeedbackService.DTO;
using FeedbackService.Models;
using FeedbackService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackService.Controllers
{


    [ApiController]
    [Route("api/[controller][action]")]
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

        [HttpGet(Name = "GetReviewsByUserId")]
        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            return await _reviewService.GetReviewsByUserId(userId);
        }
        [HttpGet(Name = "GetReviewsByRestaurantId")]
        public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _reviewService.GetReviewsByRestaurantId(restaurantId);
        }
        [HttpGet(Name = "GetReviewsByDeliveryUserId")]
        public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryDriverId)
        {
            return await _reviewService.GetReviewsByDeliveryUserId(deliveryDriverId);
        }
    }
}

