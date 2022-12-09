using DeliveryService.Models;
using DeliveryService.Repository;
using Common.ErrorModels;
using Common.Dto;

namespace DeliveryService.Services
{
    public interface IDeliverySerivce
    {
        public Task<bool> CreateDelivery(CreateDeliveryDto createDeliveryDTO);
        public Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int DeliveryPersonId);
        public Task<Delivery> GetDeliveryByOrderId(int orderId);
        public Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail);
        public Task<bool> UpdateDeliveryToIsCancelled(int orderId);
        public Task<Delivery> GetDeliveryByDeliveryId(int deliveryId);
        public Task<bool> UpdateDeliveryAsDelivered(int deliveryId);
    }

    public class DeliveryService : IDeliverySerivce
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        /// <summary>
        /// Checks if a person has a delivery...
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> CheckIfHasDelivery(int deliveryPersonId)
        {
            var delivery = await _deliveryRepository.GetDeliveryPersonWhereIsDeliveredFalse(deliveryPersonId);
            if (delivery != null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    "U cannot have more than 1 delivery at a time");
            }

            return false;
        }

        /// <summary>
        /// Creates a new delivery
        /// </summary>
        /// <param name="createDeliveryDTO"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> CreateDelivery(CreateDeliveryDto createDeliveryDTO)
        {
            if (!createDeliveryDTO.UserEmail.Contains("@"))
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    "Email must containt character '@' to be valid");
            }

            // Check if guy already has a delivery that is not delivered
            var deliveries = await _deliveryRepository.GetDeliveriesByUserEmail(createDeliveryDTO.UserEmail);

            // If theres any where they ahve a delivery that isnt delivered
            if (deliveries.Any(_ => _.IsDelivered == false))
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    $"Delivery guy with email {createDeliveryDTO.UserEmail} already has a active delivery");
            }

            var delivery = new Delivery
            {
                DeliveryPersonId = createDeliveryDTO.DeliveryPersonId,
                OrderId = createDeliveryDTO.OrderId,
                UserEmail = createDeliveryDTO.UserEmail,
                TimeToDelivery = createDeliveryDTO.TimeToDelivery
            };

            await _deliveryRepository.CreateDelivery(delivery);
            return true;
        }

        /// <summary>
        /// Gets delivery by a delivery person id
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            var deliveries = await _deliveryRepository.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
            if (!deliveries.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "U do not have any deliveries");
            }

            return deliveries;
        }

        /// <summary>
        /// Gets delivery by a user email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            var deliveries = await _deliveryRepository.GetDeliveriesByUserEmail(userEmail);
            if (!deliveries.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "U do not have any deliveries");
            }

            return deliveries;
        }

        /// <summary>
        /// Gets deliveries by a given delivery Id
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<Delivery> GetDeliveryByDeliveryId(int deliveryId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByDeliveryId(deliveryId);

            if (delivery == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Delivery does not exist");
            }

            return delivery;
        }

        /// <summary>
        /// Gets a delivery by a order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByOrderId(orderId);
            if (delivery == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    $"Order with given id = {orderId} does not exist");
            }

            return delivery;
        }

        /// <summary>
        /// Updates a given delivery to is delivered
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDeliveryAsDelivered(int deliveryId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByDeliveryId(deliveryId);

            if (delivery == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Delivery does not exist");
            }

            return await _deliveryRepository.UpdateDeliveryAsDelivered(delivery);
        }

        /// <summary>
        /// Updates a delivery to is cancelled
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> UpdateDeliveryToIsCancelled(int orderId)
        {
            var delivery = await _deliveryRepository.GetDeliveryByOrderId(orderId);
            if (delivery == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    "Delivery with given id does not exists ");
            }

            delivery.isCancelled = true;
            delivery.TimeToDelivery = DateTime.MinValue;
            await _deliveryRepository.UpdateDeliveryToIsCancelled(delivery);
            return true;
        }
    }
}