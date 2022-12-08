using Microsoft.EntityFrameworkCore;
using OrderService.Context;
using OrderService.Models;

namespace OrderService.Test.Context
{
    public class SetupInMemoryDatabase
    {
        public static OrderDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<OrderDbContext>().UseInMemoryDatabase(databaseName: "OrderServiceTest").Options;

            var context = new OrderDbContext(option);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var order1 = new Order
            {
                Id = 1,
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
                OrderId = 1,
            };

            var item2 = new OrderItem
            {
                ItemPrice = 10,
                Name = "Pizza",
                Order = order1,
                OrderId = 1,
            };

            var item3 = new OrderItem
            {
                ItemPrice = 10,
                Name = "Pizza",
                Order = order1,
                OrderId = 1,
            };

            var item4 = new OrderItem
            {
                ItemPrice = 10,
                Name = "Pizza",
                Order = order1,
                OrderId = 1,
            };

            //order1.MenuItems.Add(item1);
            //order1.MenuItems.Add(item2);
            //order1.MenuItems.Add(item3);
            //order1.MenuItems.Add(item4);

            //context.Orders.Add(order1);

            context.SaveChanges();

            return context;
        }
    }
}
