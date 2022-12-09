using DeliveryService.Repository;
using DeliveryService.Services;
using DeliveryService.DTO;
using Moq;
using DeliveryService.Models;
using FluentAssertions;
using Common.ErrorModels;
using Bogus;

namespace DeliveryService.Test
{
    public class DeliveryServiceUnittest
    {
        public Mock<IDeliveryRepository> _deliveryRepository;
        public IDeliverySerivce _deliverySerivce;

        [SetUp]
        public void Setup()
        {
            _deliveryRepository = new Mock<IDeliveryRepository>();
            _deliverySerivce = new DeliveryService.Services.DeliveryService(_deliveryRepository.Object);
        }

        /// <summary>
        /// Positive test to showcase creating a delivery with a dummy review and mock
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateDelivery_Should_Return_True_Positive()
        {
            var createDeliveryDto = new CreateDeliveryDTO
            {
                DeliveryPersonId = 1,
                OrderId = 1,
                RestaurantId = 1,
                TimeToDelivery = DateTime.MinValue,
                UserEmail = "Test@test.dk"
            };

            var dummyDelivery = new Delivery { };

            _deliveryRepository.Setup(_ => _.CreateDelivery(dummyDelivery)).ReturnsAsync(true);

            // Act
            var actualMocked = await _deliverySerivce.CreateDelivery(createDeliveryDto);


            // Assert
            actualMocked.Should().BeTrue();
            // Verify that a mock was called
            _deliveryRepository.Verify(mock => mock.CreateDelivery(It.IsAny<Delivery>()), Times.Exactly(1));
        }

        /// <summary>
        /// Negative test to show the creation of a delivery with an email not containing '@' and with dummy data and mock
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateDelivery_Should_Return_False_Negative()
        {
            var createDeliveryDto = new CreateDeliveryDTO
            {
                DeliveryPersonId = 1,
                OrderId = 1,
                RestaurantId = 1,
                TimeToDelivery = DateTime.MinValue,
                // Setting the email wihtout the '@' character
                UserEmail = "Test.test.dk"
            };

            var dummyDelivery = new Delivery { };

            _deliveryRepository.Setup(_ => _.CreateDelivery(dummyDelivery)).ReturnsAsync(false);

            // Act
            var actualMocked = false;

            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.CreateDelivery(createDeliveryDto);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeFalse();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("Email must containt character '@' to be valid");
        }

        /// <summary>
        /// Positive test to show the return of a list if the the delivery person has any deliveries (total)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveriesByDeliveryPersonId_Should_Return_list_Positive()
        {
            var deliveryPersonId = 1;

            var createDelivery = new Delivery
            {
                DeliveryId = 1,
                DeliveryPersonId = 1,
                OrderId = 1,
                RestaurantId = 1,
                UserEmail = "test@test.dk",
                TimeToDelivery = DateTime.MinValue
            };

            _deliveryRepository.Setup(_ => _.GetDeliveriesByDeliveryPersonId(deliveryPersonId)).ReturnsAsync(GenerateDummyData());
            List<Delivery> actualMocked = new List<Delivery>();

            // Act
            actualMocked = await _deliverySerivce.GetDeliveriesByDeliveryPersonId(deliveryPersonId);

            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
                await Task.CompletedTask;
            };

            actualMocked.Should().NotBeEmpty().And.HaveCount(5);

            _deliveryRepository.Verify(_ => _.GetDeliveriesByDeliveryPersonId(It.IsAny<int>()), Times.Exactly(1));

        }

        


        public static List<Delivery> GenerateDummyData()
        {
            Faker<Delivery> reviewFaker = new Faker<Delivery>()
               .StrictMode(true)
               .RuleFor(x => x.DeliveryId, x => x.Random.Int(10))
               .RuleFor(x => x.DeliveryPersonId, x => x.Random.Int(10))
               .RuleFor(x => x.OrderId, x => x.Random.Int())
               .RuleFor(x => x.UserEmail, x => x.Random.String())
               .RuleFor(x => x.RestaurantId, x => x.Random.Int())
               .RuleFor(x => x.TimeToDelivery, x => x.Date.Recent(0))
               .RuleFor(x => x.IsDelivered, false)
               .RuleFor(x => x.isCancelled, false)
               .RuleFor(x => x.CreatedDate, x => x.Date.Recent(0));
            return reviewFaker.Generate(5);
        }
    }
}