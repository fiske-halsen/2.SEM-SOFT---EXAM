using FeedbackService.Context;
using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Repository
{
    public class FeedbackRepository
    {
        public interface IReviewStorage
        {
            public Task<Review> CreateReview(Review review);
            public Task<Review> GetReviewByReviewId(int reviewId);
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

         
            public async Task<Review> CreateReview(Review review)
            {
                try
                {
                    await _dbContext.AddAsync(review);
                    await _dbContext.SaveChangesAsync();
                    return review;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        
            public async Task<Review> GetReviewByReviewId(int reviewId)
            {
                try
                {
                    var reviews = await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
                    return reviews;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public Task<List<Review>> GetReviewsByDeliveryUserId(int deliveryUserId)
            {
                throw new NotImplementedException();
            }

            public Task<List<Review>> GetReviewsByRestaurantId(int restaurantId)
            {
                throw new NotImplementedException();
            }

            public Task<List<Review>> GetReviewsByUserId(int userId)
            {
                throw new NotImplementedException();
            }
        }
}
