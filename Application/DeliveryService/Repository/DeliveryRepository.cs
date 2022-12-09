using System.Diagnostics;
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
        public Task<Delivery> GetDeliveryByDeliveryId(int deliveryId);
        public Task<bool> UpdateDeliveryAsDelivered(Delivery delivery);

    }

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DbApplicationContext _applicationContext;

        public DeliveryRepository(DbApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Creates a new delivery and saves it in the database
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public async Task<bool> CreateDelivery(Delivery delivery)
        {
            _applicationContext.Deliveries.Add(delivery);
            await _applicationContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _applicationContext.Deliveries.Where(x => x.DeliveryPersonId == deliveryPersonId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all deliveries by user email from the database
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _applicationContext.Deliveries.Where(x => x.UserEmail == userEmail).ToListAsync();
        }

        /// <summary>
        /// Gets all deliveries by a deliveryPersonId that arent delivered
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        public async Task<Delivery> GetDeliveryPersonWhereIsDeliveredFalse(int deliveryPersonId)
        {
            return await _applicationContext.Deliveries
                .Where(x => x.DeliveryPersonId == deliveryPersonId && !x.IsDelivered).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a delivery by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            return await _applicationContext.Deliveries.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates delivery to is cancelled in the database
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDeliveryToIsCancelled(Delivery delivery)
        {
            try
            {
                _applicationContext.Deliveries.Update(delivery);
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets a delivery by delivery id from the database
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Delivery> GetDeliveryByDeliveryId(int deliveryId)
        {
            return _applicationContext.Deliveries.Where(_ => _.DeliveryId == deliveryId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the status of a delivery to delivered
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateDeliveryAsDelivered(Delivery delivery)
        {
            try
            {
                _applicationContext.Deliveries.Update(delivery);
                await _applicationContext.SaveChangesAsync();
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