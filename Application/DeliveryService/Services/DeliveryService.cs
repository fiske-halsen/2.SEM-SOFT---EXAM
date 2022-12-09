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

        public Task<bool> UpdateDeliveryToIsCancelled(int OrderId);
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

        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            var deliveries = await _deliveryRepository.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
            if (!deliveries.Any()) { throw new HttpStatusException(StatusCodes.Status400BadRequest, "U do not have any deliveries"); }
            return deliveries;
        }

        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            var deliveries = await _deliveryRepository.GetDeliveriesByUserEmail(userEmail);
            if (!deliveries.Any()) { throw new HttpStatusException(StatusCodes.Status400BadRequest, "U do not have any deliveries"); }
            return deliveries;
        }

        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByOrderId(orderId);
            if(delivery == null) { throw new HttpStatusException(StatusCodes.Status400BadRequest, $"Order with given id = {orderId} does not exist"); }
            return delivery;
        }

        public async Task<bool> UpdateDeliveryToIsCancelled(int OrderId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByOrderId(OrderId);
            if (delivery == null) { return false; }
            delivery.isCancelled = true;
            delivery.TimeToDelivery = DateTime.MinValue;
            await _deliveryRepository.UpdateDeliveryToIsCancelled(delivery);
            return true;
        }
    }
}
