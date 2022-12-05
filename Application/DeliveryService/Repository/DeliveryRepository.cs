using DeliveryService.Context;
using DeliveryService.DTO;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Repository
{
    public interface IDeliveryRepository { 
        public Task<bool> CreateDelivery(Delivery delivery);
        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId);
        public Task<Delivery> GetDeliveryByOrderId(int OrderId);
        public Task<List<Delivery>> GetDeliveriesByUserEmail(string UserEmail);
        public Task<List<Delivery>> GetDeliveriesByZipCode(string ZipCode);
        public Task<Delivery> GetDeliveryPersonWhereIsDeliveredFalse(int deliverPersonId);
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
            try
            {
                _applicationContext.Deliveries.Add(delivery);
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Delivery>> GetDeliveriesByUserEmail(string UserEmail)
        {
            throw new NotImplementedException();
        }

        public Task<List<Delivery>> GetDeliveriesByZipCode(string ZipCode)
        {
            throw new NotImplementedException();
        }

        public async Task<Delivery> GetDeliveryPersonWhereIsDeliveredFalse(int DeliveryPersonId)
        {
            return await _applicationContext.Deliveries.Where(x => x.DeliveryPersonId == DeliveryPersonId && !x.IsDelivered).FirstOrDefaultAsync();            
        }

        public Task<Delivery> GetDeliveryByOrderId(int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
