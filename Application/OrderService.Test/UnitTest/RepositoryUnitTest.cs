using FluentAssertions;
using OrderService.Context;
using OrderService.Models;
using OrderService.Repository;
using OrderService.Test.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Test.UnitTest
{
    public class RepositoryUnitTest
    {
        private OrderDbContext _context;
        private OrderRepository _orderRepository;

        [OneTimeSetUp]
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
                Id = 12,
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

        
        public async Task AcceptOrderRepository()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
