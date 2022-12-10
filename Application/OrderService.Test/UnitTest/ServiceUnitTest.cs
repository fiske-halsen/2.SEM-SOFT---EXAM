using Common.Dto;
using Common.Enums;
using FluentAssertions;
using Moq;
using OrderService.Models;
using OrderService.Repository;
using OrderService.Services;

namespace OrderService.Test.UnitTest
{
    public class ServiceUnitTest
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
                MenuItems = new List<CreateOrderMenuItemDto>
                {
                    new CreateOrderMenuItemDto { Id = 1, Price = 20 },
                    new CreateOrderMenuItemDto { Id = 1, Price = 23 }
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
        public async Task DeleteOrderTest()
        {
            //Arrange
            var orderId = 1;

            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.DeleteOrder(dummyOrder)).ReturnsAsync(true);
            _orderRepositoryMock.Setup(_ => _.GetOrderById(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.DeleteOrder(orderId);

            //Assert
            _orderRepositoryMock.Verify(mock => mock.DeleteOrder(dummyOrder), Times.Exactly(1));
        }

        [Test]
        public async Task AcceptOrderTest()
        {
            //Arrange
            var orderId = 1;

            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.AcceptOrder(orderId)).ReturnsAsync(true);
            _orderRepositoryMock.Setup(_ => _.GetOrderById(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.AcceptOrder(orderId);

            //Assert
            actualMocked.Should().Be(true);
            _orderRepositoryMock.Verify(mock => mock.AcceptOrder(1), Times.Exactly(1));
        }

        [Test]
        public async Task GetAllOrdersForRestaurantTest()
        {
            //Arrange
            var restaurantId = 1;
            var isApproved = true;
            var orders = new List<Order>();

            var order1 = new Order
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsApproved = false,
                TotalPrice = 40,
                CardType = CardTypes.Visa,
                CustomerEmail = "test@test.dk",
                PaymentType = PaymentTypes.CreditCard,
                RestaurantId = 1,
                MenuItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = restaurantId,
                        ItemPrice = 23,
                        MenuItemId = 1,
                        OrderId = 1
                    }
                },
            };

            var order2 = new Order
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsApproved = false,
                TotalPrice = 40,
                CardType = CardTypes.Visa,
                CustomerEmail = "test@test.dk",
                PaymentType = PaymentTypes.CreditCard,
                RestaurantId = 1,
                MenuItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = restaurantId,
                        ItemPrice = 23,
                        MenuItemId = 1,
                        OrderId = 1
                    }
                },
            };

            orders.Add(order1);
            orders.Add(order2);


            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.GetAllOrdersForRestaurant(isApproved, restaurantId)).ReturnsAsync(orders);
            _orderRepositoryMock.Setup(_ => _.GetAllOrdersForRestaurant(restaurantId)).ReturnsAsync(orders);

            //Act
            var actualMocked1 = await _orderService.GetAllOrdersForRestaurant(isApproved, restaurantId);
            var actualMocked2 = await _orderService.GetAllOrdersForRestaurant(restaurantId);

            //Assert
            actualMocked1.Should().BeOfType<List<Order>>();
            actualMocked2.Should().BeOfType<List<Order>>();
            _orderRepositoryMock.Verify(mock => mock.GetAllOrdersForRestaurant(true, 1), Times.Exactly(1));
            _orderRepositoryMock.Verify(mock => mock.GetAllOrdersForRestaurant(1), Times.Exactly(1));
        }

        [Test]
        public async Task GetAllOrdersForUserTest()
        {
            //Arrange
            var userEmail = "test@test.dk";

            var order1 = new List<Order>
            {
                new Order
                {
                Id = 1,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsApproved = false,
                TotalPrice = 40,
                CardType = CardTypes.Visa,
                CustomerEmail = "test@test.dk",
                PaymentType = PaymentTypes.CreditCard,
                RestaurantId = 1,
                MenuItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 1,
                        ItemPrice = 23,
                        MenuItemId = 1,
                        OrderId = 1
                    }
                }
                }
            };

            _orderRepositoryMock.Setup(_ => _.GetOrdersForUser(userEmail)).ReturnsAsync(order1);

            //Act
            var actualMocked = await _orderService.GetAllOrdersForUser(userEmail);

            //Assert
            actualMocked.Should().BeOfType<List<Order>>();
            _orderRepositoryMock.Verify(mock => mock.GetOrdersForUser(userEmail), Times.Exactly(1));
        }

        [Test]
        public async Task UpdateOrderSetInActiveTest()
        {
            //Arrange
            var orderId = 1;

            var dummyOrder = new Order { };
            _orderRepositoryMock.Setup(_ => _.UpdateOrderSetInActive(dummyOrder)).ReturnsAsync(true);
            _orderRepositoryMock.Setup(_ => _.GetOrderById(orderId)).ReturnsAsync(dummyOrder);

            //Act
            var actualMocked = await _orderService.UpdateOrderSetInActive(orderId);

            //Assert
            actualMocked.Should().BeTrue();
        }
    }
}