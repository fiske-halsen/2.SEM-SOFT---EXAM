using FeedbackService.Context;
using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Repository
{
    public interface IReviewRepository
    {
        public Task<bool> CreateReview(Review review);
        public Task<List<Review>> GetReviewsByUserId(int userId);
        public Task<List<Review>> GetReviewsByRestaurantId(int restaurantId);
        public Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId);
    }

    /// <summary>
    /// Review repository contains the logic for communicating withe review db
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        private readonly DBFeedbackServiceContext _dbContext;

        public ReviewRepository(DBFeedbackServiceContext dBApplicationContext)
        {
            _dbContext = dBApplicationContext;
        }

        /// <summary>
        /// Create a new review
        /// </summary>
        /// <param name="review"></param>
        /// <returns>true</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CreateReview(Review review)
        {
            try
            {
                await _dbContext.AddAsync(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get reviews for a delivery driver
        /// </summary>
        /// <param name="deliveryUserId"></param>
        /// <returns>reviews</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId)
        {
            try
            {

                var reviews = await _dbContext.Reviews.Where(x => x.DeliveryDriverId == deliveryUserId).ToListAsync();
                return reviews;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get reviews for a restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns>reviews</returns>
        /// <exception cref="Exception"></exception>
        /// 
        public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
        {
            try
            {

                var reviews = await _dbContext.Reviews.Where(x => x.RestaurantId == restaurantId).ToListAsync();
                return reviews;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get reviews for a user 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>reviews</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            try
            {

                var reviews = await _dbContext.Reviews.Where(_ => _.UserId == userId).ToListAsync();
                return reviews;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

