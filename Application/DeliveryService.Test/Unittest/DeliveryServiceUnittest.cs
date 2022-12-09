using DeliveryService.Repository;
using DeliveryService.Services;
using Moq;
using DeliveryService.Models;
using FluentAssertions;
using Common.ErrorModels;
using Bogus;
using Common.Dto;

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
            var createDeliveryDto = new CreateDeliveryDto()
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
            var createDeliveryDto = new CreateDeliveryDto()
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

        /// <summary>
        /// Positive test to show that nothing is returned if a deliveryPerson does not have any deliveries in total(delivered or not)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveriesByDeliveryPersonId_Should_Not_Return_list_Negative()
        {
            var deliveryPersonId = 2;

            _deliveryRepository.Setup(_ => _.GetDeliveriesByDeliveryPersonId(deliveryPersonId)).ReturnsAsync(GenerateDummyData());
            List<Delivery> actualMocked = new List<Delivery>();

            // Act
            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
                await Task.CompletedTask;
            };
            // Assert
            actualMocked.Should().BeEmpty().And.HaveCount(0);
        }

        /// <summary>
        /// Positive test to show that a Delivery should be returned if a delivery with a matching orderId exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveryByOrderId_Should_Return_Delivery_Positive()
        {
            var orderId = 1;

            var delivery = new Delivery()
            {
                DeliveryId = 1,
                DeliveryPersonId = 1,
                OrderId = orderId,
                RestaurantId = 1,
                UserEmail = "test@test.dk",
                TimeToDelivery = DateTime.MinValue
            };

            _deliveryRepository.Setup(_ => _.GetDeliveryByOrderId(orderId)).ReturnsAsync(delivery);

            // Act
            var actualMocked = await _deliverySerivce.GetDeliveryByOrderId(orderId);
            var expected = delivery;

            // Assert
            actualMocked.Should().BeOfType<Delivery>();
            actualMocked.Should().BeEquivalentTo(delivery, options => options.ExcludingMissingMembers());

            // Verify using mock
            _deliveryRepository.Verify(mock => mock.GetDeliveryByOrderId(It.IsAny<int>()), Times.Exactly(1));
        }

        /// <summary>
        /// Negative test to show that a Delivery should not be returned if an orderId of a non existsing delivery is given
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveryByOrderId_Should_Not_Return_Delivery_Negative()
        {
            var orderId = 1;

            var delivery = new Delivery()
            {
                DeliveryId = 1,
                DeliveryPersonId = 1,
                OrderId = orderId,
                RestaurantId = 1,
                UserEmail = "test@test.dk",
                TimeToDelivery = DateTime.MinValue
            };

            _deliveryRepository.Setup(_ => _.GetDeliveryByOrderId(orderId)); // setup assuming email already exists


            // Act
            Delivery actualMocked = null;

            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.GetDeliveryByOrderId(orderId);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeNull();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage($"Order with given id = {orderId} does not exist");
        }

        /// <summary>
        /// Positive test to show that a list og Delivery should be returned if a email with deliveries exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveriesByUserEmail_Should_Return_Delivery_Positive()
        {
            var userEmail = "test@test.dk";

            var createDelivery = new Delivery
            {
                DeliveryId = 1,
                DeliveryPersonId = 1,
                OrderId = 1,
                RestaurantId = 1,
                UserEmail = userEmail,
                TimeToDelivery = DateTime.MinValue
            };

            _deliveryRepository.Setup(_ => _.GetDeliveriesByUserEmail(userEmail)).ReturnsAsync(GenerateDummyData());
            List<Delivery> actualMocked = new List<Delivery>();

            // Act
            actualMocked = await _deliverySerivce.GetDeliveriesByUserEmail(userEmail);

            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.GetDeliveriesByUserEmail(userEmail);
                await Task.CompletedTask;
            };

            actualMocked.Should().NotBeEmpty().And.HaveCount(5);

            _deliveryRepository.Verify(_ => _.GetDeliveriesByUserEmail(It.IsAny<string>()), Times.Exactly(1));

        }

        /// <summary>
        /// Negative test to show that a list og Delivery should not be returned if a email with no deliveries exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDeliveriesByUserEmail_Should_Not_Return_Delivery_Negative()
        {
            var userEmail = "asd@asd.dk";

            _deliveryRepository.Setup(_ => _.GetDeliveriesByUserEmail(userEmail)).ReturnsAsync(GenerateDummyData());
            List<Delivery> actualMocked = new List<Delivery>();

            // Act
            Func<Task> act = async () =>
            {
                actualMocked = await _deliverySerivce.GetDeliveriesByUserEmail(userEmail);
                await Task.CompletedTask;
            };
            // Assert
            actualMocked.Should().BeEmpty().And.HaveCount(0);
        }

        // MISSING
        //[Test]
        //public async Task UpdateDeliveryToIsCancelled_Should_Return_True_Positive()
        //{
        //    var orderId = 1;

        //    var delivery = new Delivery()
        //    {
        //        DeliveryId = 1,
        //        DeliveryPersonId = 1,
        //        OrderId = orderId,
        //        RestaurantId = 1,
        //        UserEmail = "test@test.dk",
        //        TimeToDelivery = DateTime.MinValue,
        //        CreatedDate = DateTime.Now,
        //        isCancelled = false,
        //        IsDelivered = false
        //    };

        //    var dummyDelivery = new Delivery { };

        //    _deliveryRepository.Setup(_ => _.CreateDelivery(delivery)).ReturnsAsync(true);

        //    // Act
        //    var actualMocked = await _deliverySerivce.UpdateDeliveryToIsCancelled(orderId);


        //    // Assert
        //    actualMocked.Should().BeTrue();
        //    // Verify that a mock was called
        //    _deliveryRepository.Verify(mock => mock.UpdateDeliveryToIsCancelled(It.IsAny<Delivery>()), Times.Exactly(1));
        //}

        // MISSING
        //[Test]
        //public async Task UpdateDeliveryToIsCancelled_Should_Not_Return_True_Negative()
        //{
        //}


        public static List<Delivery> GenerateDummyData()
        {
            Faker<Delivery> reviewFaker = new Faker<Delivery>()
               .StrictMode(true)
               .RuleFor(x => x.DeliveryId, x => x.Random.Int(10))
               .RuleFor(x => x.DeliveryPersonId,1)
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