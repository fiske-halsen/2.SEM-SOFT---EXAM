using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OrderService.Context;
using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        public Task<Order> AcceptOrder(int id);
        public Task<Order> CancelOrder(int id);
        public Task<bool> CreateOrder(Order order);
        public Task<Order> DenyOrder(int id);
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

        public async Task<Order> CancelOrder(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public async Task<Order> AcceptOrder(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
                order.IsApproved = true;
                await _dbContext.SaveChangesAsync();
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public async Task<Order> DenyOrder(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
                order.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}