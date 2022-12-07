using Microsoft.EntityFrameworkCore;
using OrderService.Context;
using OrderService.Models;
using System.Diagnostics;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        public Task<bool> AcceptOrder(int orderId);
        public Task<bool> DeleteOrder(Order order);
        public Task<bool> CreateOrder(Order order);
        public Task<Order> GetOrderById(int orderId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new order and saves it in the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a order by id in the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(Order order)
        {
            try
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Accepts a given order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<bool> AcceptOrder(int orderId)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                order.IsApproved = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Gets a order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }
    }
}