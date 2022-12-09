using FluentAssertions;
using OrderService.Context;
using OrderService.Models;
using OrderService.Repository;
using OrderService.Test.Context;

namespace OrderService.Test.UnitTest
{
    public class RepositoryUnitTest
    {
        private readonly OrderDbContext _context;
        private readonly OrderRepository _orderRepository;

        [SetUp]
        public void Setup()
        {
            _context = SetupInMemoryDatabase.CreateContextForInMemory();
            _orderRepository = new OrderRepository(_context);
        }

        [Test]
        public async Task CreateOrderRepository()
        {
            //Arrange
            var order1 = new Order
            {
                Id = 15,
                CustomerEmail = "test@test.dk",
                IsActive = true,
                IsApproved = false,
                MenuItems = new List<OrderItem> { },
                RestaurantId = 1,
                TimeToDelivery = 1030,
                TotalPrice = 45
            };

            var item1 = new OrderItem
            {
                Id = 11,
                ItemPrice = 10,
                Name = "Pizza",
                Order = order1,
                OrderId = 12,
            };

            order1.MenuItems.Add(item1);

            //Act
            var response = await _orderRepository.CreateOrder(order1);

            //Assert
            response.Should().BeTrue();
        }
        
        [Test]
        public async Task AcceptOrderRepository()
        {
            //Arrange
            var orderId = 1;

            //Act
            var response = await _orderRepository.AcceptOrder(orderId);

            //Assert
            response.Should().BeOfType<Order>();
        }

        [Test]
        public async Task DenyOrderRepository()
        {
            //Arrange
            var orderId = 2;

            //Act
            var response = await _orderRepository.DenyOrder(orderId);

            //Assert
            response.Should().BeOfType<Order>();
        }

        [Test]
        public async Task CancelOrderRepository()
        {
            //Arrange
            var orderId = 1;

            //Act
            var response = await _orderRepository.CancelOrder(orderId);

            //Assert
            response.Should().BeOfType<Order>();
        }
    }
}
