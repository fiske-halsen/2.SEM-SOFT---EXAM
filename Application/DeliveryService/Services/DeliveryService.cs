using DeliveryService.DTO;
using DeliveryService.Models;
using DeliveryService.Repository;
using Common.ErrorModels;

namespace DeliveryService.Services
{
    public interface IDeliverySerivce
    { 
        public Task<bool> CreateDelivery(CreateDeliveryDTO createDeliveryDTO);
        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId);
        public Task<Delivery> GetDeliveryByOrderId(int OrderId);
        public Task<List<Delivery>> GetDeliveriesByUserEmail(string UserEmail);
        public Task<List<Delivery>> GetDeliveriesByZipCode(string ZipCode);
        public Task<List<Delivery>> GetDeliveriesIfDelivered();
        public Task<List<Delivery>> GetDeliveriesNotDelivered();

    }
    public class DeliveryService : IDeliverySerivce
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task <bool> CheckIfHasDelivery(int deliveryPersonId)
        {
            var delivery = await _deliveryRepository.GetDeliveryPersonWhereIsDeliveredFalse(deliveryPersonId);
            if(delivery != null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "U cannot have more than 1 delivery at a time");
            }
            return false;
        }

        public async Task<bool> CreateDelivery(CreateDeliveryDTO createDeliveryDTO)
        {


            var delivery = new Delivery
            {
                DeliveryPersonId = createDeliveryDTO.DeliveryPersonId,
                OrderId = createDeliveryDTO.OrderId,
                UserEmail = createDeliveryDTO.UserEmail
            };
            
            
            await _deliveryRepository.CreateDelivery(delivery);
            return true;
        }

        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<List<Delivery>> GetDeliveriesByZipCode(string zipCode)
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

        public Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
