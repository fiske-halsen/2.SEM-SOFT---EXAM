using DeliveryService.DTO;
using DeliveryService.Models;
using DeliveryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliverySerivce _deliverySerivce;

        public DeliveryController(IDeliverySerivce deliverySerivce)
        {
            _deliverySerivce = deliverySerivce;
        }

        [HttpPost]
        public async Task<bool> CreateDelivery([FromBody]CreateDeliveryDTO createDeliveryDTO)
        {
            return await _deliverySerivce.CreateDelivery(createDeliveryDTO);
        }

        [HttpGet("delivery-persons/{deliveryPersonId}")]
        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _deliverySerivce.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            return await _deliverySerivce.GetDeliveryByOrderId(orderId);
        }
        [HttpGet("customers/{userEmail}")]
        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _deliverySerivce.GetDeliveriesByUserEmail(userEmail);
        }

        [HttpPatch("orders/cancel/{orderId}")]
        public async Task<bool> UpdateDeliveryToIsCancelled(int orderId)
        {
            return await _deliverySerivce.UpdateDeliveryToIsCancelled(orderId);
        }
    }
}
