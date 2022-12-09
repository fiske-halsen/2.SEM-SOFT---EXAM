using Common.Dto;
using DeliveryService.Models;
using DeliveryService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliverySerivice _deliverySerivice;

        public DeliveryController(IDeliverySerivice deliverySerivice)
        {
            _deliverySerivice = deliverySerivice;
        }

        [Authorize]
        [HttpPost]
        public async Task<bool>
            CreateDelivery(
                [FromBody]
                CreateDeliveryDto createDeliveryDTO) // This method should not be http based, we are making this event based for real time deliveries
        {
            return await _deliverySerivice.CreateDelivery(createDeliveryDTO);
        }

        [Authorize]
        [HttpGet("delivery-persons/{deliveryPersonId}")]
        public async Task<List<Delivery>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _deliverySerivice.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
        }

        [Authorize]
        [HttpGet("orders/{orderId}")]
        public async Task<Delivery> GetDeliveryByOrderId(int orderId)
        {
            return await _deliverySerivice.GetDeliveryByOrderId(orderId);
        }

        [Authorize]
        [HttpGet("customers/{userEmail}")]
        public async Task<List<Delivery>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _deliverySerivice.GetDeliveriesByUserEmail(userEmail);
        }

        [Authorize]
        [HttpPatch("orders/cancel/{orderId}")]
        public async Task<bool> UpdateDeliveryToIsCancelled(int orderId)
        {
            return await _deliverySerivice.UpdateDeliveryToIsCancelled(orderId);
        }
    }
}