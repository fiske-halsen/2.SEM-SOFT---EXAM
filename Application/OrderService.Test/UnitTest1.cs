using Common.Dto;
using Common.Enums;
using FluentAssertions;
using Moq;
using OrderService.Models;
using OrderService.Repository;
using OrderService.Services;

namespace OrderService.Test
{
    public class Tests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrdersService(_orderRepositoryMock.Object);
        }

        [Test]
        public async Task CreateOrderTest()
        {
            //Arrange
            var order = new CreateOrderDto
            {
                CardType = CardTypes.Visa,
                CustomerEmail = "test@test.dk",
                MenuItems = new List<MenuItemDTO>
                {
                    new MenuItemDTO { Id = 1, description = "food", name = "pizza", price = 10.10, StockCount = 10 }
                },
                OrderTotal = 10,
                PaymentType = PaymentTypes.CreditCard,
                RestaurantId = 1,
                VoucherCode = "fifty%OffFood"
            };

            var dummyOrder = new Order 
            {
                
            };

            _orderRepositoryMock.Setup(_ => _.CreateOrder(dummyOrder)).ReturnsAsync(true);

            //Act
            var actualMocked = await _orderService.CreateOrder(order);

            //Assert
            actualMocked.Should().BeTrue();

            //Verify mock call
            _orderRepositoryMock.Verify(mock => mock.CreateOrder(It.IsAny<Order>()), Times.Exactly(1));
        }

        [Test]
        public async Task CancelOrderTest()
        {
            //Arrange
            var orderId = 1;
            
            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.CancelOrder(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.CancelOrder(orderId);

            //Assert
            _orderRepositoryMock.Verify(mock => mock.CancelOrder(1), Times.Exactly(1));
        }

        [Test]
        public async Task TimeToDeliveryTest()
        {
            //Arrange
            var orderId = 1;

            _orderRepositoryMock.Setup(_ => _.TimeToDelivery(orderId)).ReturnsAsync(1030);

            //Act
            var actualMocked = await _orderService.TimeToDelivery(orderId);

            //Assert
            actualMocked.Should().Be(1030);
            _orderRepositoryMock.Verify(mock => mock.TimeToDelivery(1), Times.Exactly(1));
        }

        [Test]
        public async Task AcceptOrderTest()
        {
            //Arrange
            var orderId = 1;

            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.AcceptOrder(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.AcceptOrder(orderId);

            //Assert
            actualMocked.Should().Be(dummyOrder);
            _orderRepositoryMock.Verify(mock => mock.AcceptOrder(1), Times.Exactly(1));
        }

        [Test]
        public async Task DenyOrderTest()
        {
            //Arrange
            var orderId = 1;

            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.DenyOrder(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.DenyOrder(1);

            //Assert
            actualMocked.Should().Be(dummyOrder);
            _orderRepositoryMock.Verify(mock => mock.DenyOrder(1), Times.Exactly(1));
        }
    }
}