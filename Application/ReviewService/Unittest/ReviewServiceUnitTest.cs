using Bogus;
using Common.ErrorModels;
using FeedbackService.DTO;
using FeedbackService.Models;
using FeedbackService.Repository;
using FeedbackService.Services;
using FluentAssertions;
using Moq;


namespace ReviewServiceTest.UnitTest
{
    public class ReviewServiceUnitTest
    {
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private IReviewService _reviewService;


        [SetUp]
        public void Setup()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviewService = new FeedbackService.Services.ReviewService(_reviewRepositoryMock.Object);
        }

        /// <summary>
        /// Positive test to showcase creating a review with a dummy review and mock
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateReview_Should_Return_True_If_Succesfull()
        {
            //Arrange
            var createReviewDto = new CreateReviewDTO
            {
                UserId = 1,
                RestaurantId = 2,
                DeliveryDriverId = 3,
                OrderId = 4,
                ReviewText = "Very good food and nice delivery guy",
                Rating = 5
            };

            var dummyReview = new Review { };

            _reviewRepositoryMock.Setup(_ => _.CreateReview(dummyReview)).ReturnsAsync(true);

            // Act
            var actualMocked = await _reviewService.CreateReview(createReviewDto);
            var expected = true;


            // Assert
            actualMocked.Should().BeTrue();
            // Verify that a mock was called
            _reviewRepositoryMock.Verify(mock => mock.CreateReview(It.IsAny<Review>()), Times.Exactly(1));


        }
        /// <summary>
        /// Negative test to show the creation of a review with a rating higher than 5 with dummy data and mock
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateReview_Should_Return_False_If_Not_Created()
        {
            //Arrange
            var createReviewDto = new CreateReviewDTO
            {
                UserId = 1,
                RestaurantId = 2,
                DeliveryDriverId = 3,
                OrderId = 4,
                ReviewText = "Very good food and nice delivery guy",
                //Setting the rating higher han 5 to fail the test
                Rating = 6
            };

            var dummyReview = new Review { };

            _reviewRepositoryMock.Setup(_ => _.CreateReview(dummyReview)).ReturnsAsync(true);

            // Act
            var actualMocked = false;

            Func<Task> act = async () =>
            {
                actualMocked = await _reviewService.CreateReview(createReviewDto);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeFalse();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("Rating cant be higher than 5");


        }
        /// <summary>
        /// Positive test to show the return of a list if the delivery driver is in the database
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetReviewsByDeliveryUserId_Should_Return_List_Positive()
        {
            // Arrange
            var DeliveryDriverId = 3;

            var createReview = new Review
            {
                UserId = 1,
                RestaurantId = 2,
                DeliveryDriverId = 3,
                OrderId = 4,
                ReviewText = "Very good food and nice delivery guy",
                Rating = 5
            };

            _reviewRepositoryMock.Setup(_ => _.GetReviewsByDeliveryUserId(DeliveryDriverId)).ReturnsAsync(GenerateDummyData());
            List<Review> actualMocked = new List<Review>();
            // Act
            actualMocked = await _reviewService.GetReviewsByDeliveryUserId(DeliveryDriverId);

            Review[] expected = { createReview };

            // Assert
            actualMocked.Should().NotBeEmpty().And.HaveCount(5);

            // Verify using mock
            _reviewRepositoryMock.Verify(_ => _.GetReviewsByDeliveryUserId(It.IsAny<int>()), Times.Exactly(1));

        }

        public static List<Review> GenerateDummyData()
        {
            Faker<Review> reviewFaker = new Faker<Review>()
               .StrictMode(true)
               .RuleFor(x => x.Id, x => x.Random.Int(10))
               .RuleFor(x => x.UserId, x => x.Random.Int(10))
               .RuleFor(x => x.RestaurantId, x => x.Random.Int())
               .RuleFor(x => x.OrderId, x => x.Random.Int())
               .RuleFor(x => x.DeliveryDriverId, x => x.Random.Int())
               .RuleFor(x => x.ReviewText, x => x.Random.String())
               .RuleFor(x => x.ReviewDate, x => x.Date.Recent(0))
               .RuleFor(x => x.Rating, x => x.Random.Int(5));

            return reviewFaker.Generate(5);
        }
    }
}
