using FluentAssertions;
using OrderService.Context;
using OrderService.Models;
using OrderService.Repository;
using OrderService.Test.Context;

namespace OrderService.Test.UnitTest
{
    public class RepositoryUnitTest
    {
        private OrderDbContext _context;
        private OrderRepository _orderRepository;

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
                TotalPrice = 45
            };

            var item1 = new OrderItem
            {
                Id = 11,
                ItemPrice = 10,
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
            response.Should().BeTrue();
        }

        [Test]
        public async Task DeleteOrderRepository()
        {
            //Arrange
            var dummyOrder = new Order { };

            _context.Orders.Add(dummyOrder);

            //Act
            var response = await _orderRepository.DeleteOrder(dummyOrder);

            //Assert
            response.Should().BeTrue();
        }

        [Test]
        public async Task GetOrderRepository()
        {
            //Arrange
            var orderId = 1;

            //Act
            var response = await _orderRepository.GetOrderById(orderId);

            //Assert
            response.Should().BeOfType<Order>();
        }

        [Test]
        public async Task GetAllOrdersRepository()
        {
            var isApproved = true;
            var restaurantId = 1;

            var response1 = await _orderRepository.GetAllOrdersForRestaurant(isApproved, restaurantId);
            var response2 = await _orderRepository.GetAllOrdersForRestaurant(restaurantId);

            response1.Should().BeOfType<List<Order>>();
            response2.Should().BeOfType<List<Order>>();
        }

        [Test]
        public async Task GetOrdersForUser()
        {
            var userEmail = "test@test.dk";

            var response = await _orderRepository.GetOrdersForUser(userEmail);

            response.Should().BeOfType<List<Order>>();
        }

        [Test]
        public async Task UpdateOrderSetInActive()
        {
            var order = new Order
            {
                Id = 2,
                CustomerEmail = "test@test.dk",
                IsActive = false,
                IsApproved = false,
                MenuItems = new List<OrderItem> { },
                RestaurantId = 1,
                TotalPrice = 45
            };

            order.IsActive = true;

            var response = await _orderRepository.UpdateOrderSetInActive(order);

            response.Should().BeTrue();
        }
    }
}
