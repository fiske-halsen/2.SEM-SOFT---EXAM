using FeedbackService.Context;
using FeedbackService.DTO;
using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Repository
{
    public class FeedbackRepository
    {
        public interface IReviewStorage
        {
            public Task<bool> CreateReview(Review review);
            public Task<List<Review>> GetReviewsByUserId(int userId);
            public Task<List<Review>> GetReviewsByRestaurantId(int restaurantId);
            public Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId);
        }

        public class BookingStorage : IReviewStorage
        {
            private readonly DBFeedbackServiceContext _dbContext;

            public BookingStorage(DBFeedbackServiceContext dBApplicationContext)
            {
                _dbContext = dBApplicationContext;
            }


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

            public async Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId)
            {
                try
                {

                    var reviews = await _dbContext.Reviews.Where(x => x.Id == deliveryUserId).ToListAsync();
                    return reviews;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public async Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
            {
                try
                {

                    var reviews = await _dbContext.Reviews.Where(x => x.Id == restaurantId).ToListAsync();
                    return reviews;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public async Task<List<Review>> GetReviewsByUserId(int userId)
            {
                try
                {

                    var reviews = await _dbContext.Reviews.Where(x => x.Id == userId).ToListAsync();
                    return reviews;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
