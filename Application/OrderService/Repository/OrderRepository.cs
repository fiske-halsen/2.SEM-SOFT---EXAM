using System.Diagnostics;
using OrderService.Context;
using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        public Task<bool> CreateOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateOrder(Order order)
        {
            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}