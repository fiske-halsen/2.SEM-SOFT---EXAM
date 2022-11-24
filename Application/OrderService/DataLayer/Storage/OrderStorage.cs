using OrderService.DataLayer.Context;
using OrderService.Models;

namespace OrderService.DataLayer.Storage
{
    public interface IOrderStorage
    {
        Task<int> createOrder(Order order);
    }

    public class OrderStorage : IOrderStorage
    {
        private readonly DbOrderServiceContext _context;

        public OrderStorage(DbOrderServiceContext context)
        {
            _context = context;
        }
        public async Task<int> createOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
