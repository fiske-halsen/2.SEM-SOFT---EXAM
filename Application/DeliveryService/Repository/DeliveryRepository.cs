using DeliveryService.Context;
using DeliveryService.DTO;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Repository
{
    public interface IDeliveryRepository
    {
        public Task<bool> CreateDelivery(Delivery delivery);
        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId);
        public Task<Delivery> GetDeliveryByOrderId(int OrderId);
        public Task<List<Delivery>> GetDeliveriesByUserEmail(string UserEmail);
        public Task<Delivery> GetDeliveryPersonWhereIsDeliveredFalse(int deliverPersonId);
        public Task<bool> UpdateDeliveryToIsCancelled(Delivery delivery);
    }
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DbApplicationContext _applicationContext;

        public DeliveryRepository(DbApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<bool> CreateDelivery(Delivery delivery)
        {
                
                _applicationContext.Deliveries.Add(delivery);
                await _applicationContext.SaveChangesAsync();
                return true;
        }

        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _applicationContext.Deliveries.Where(x => x.DeliveryPersonId == deliveryPersonId).ToListAsync();
        }

        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _applicationContext.Deliveries.Where(x => x.UserEmail == userEmail).ToListAsync();
        }

        public async Task<Delivery> GetDeliveryPersonWhereIsDeliveredFalse(int deliveryPersonId)
        {
            return await _applicationContext.Deliveries.Where(x => x.DeliveryPersonId == deliveryPersonId && !x.IsDelivered).FirstOrDefaultAsync();
        }

        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            return await _applicationContext.Deliveries.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateDeliveryToIsCancelled(Delivery delivery)
        {
            _applicationContext.Deliveries.Update(delivery);
            await _applicationContext.SaveChangesAsync();
            return true;
        }
    }
}
