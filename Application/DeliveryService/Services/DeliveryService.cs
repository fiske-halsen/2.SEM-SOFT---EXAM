using DeliveryService.Models;

namespace DeliveryService.Services
{
    public interface IDeliverSerivce
    {
        //public Task<Delivery> GetDeliveryById(int DeliveryId);
        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId);
        public Task<Delivery> GetDeliveryByOrderId(int OrderId);
        public Task<List<Delivery>> GetDeliveriesByUserEmail(string UserEmail);
        public Task<List<Delivery>> GetDeliveriesByZipCode(string ZipCode);
        public Task<List<Delivery>> GetDeliveriesIfDelivered();
        public Task<List<Delivery>> GetDeliveriesNotDelivered();


    }
    public class DeliveryService : IDeliverSerivce
    {
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

        public Task<List<Delivery>> GetDeliveriesIfDelivered()
        {
            throw new NotImplementedException();
        }

        public Task<List<Delivery>> GetDeliveriesNotDelivered()
        {
            throw new NotImplementedException();
        }

        public Task<Delivery> GetDeliveryByOrderId(int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
