using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("/restaurants/{restaurantId}")]
        public async Task<List<Order>> GetOrdersForRestaurants(int restaurantId)
        {
            return await _orderService.GetAllOrdersForRestaurant(restaurantId);
        }

        [Authorize]
        [HttpGet("/restaurants/{restaurantId}/{isApproved}")]
        public async Task<List<Order>> GetOrdersForRestaurants(int restaurantId, bool isApproved)
        {
            return await _orderService.GetAllOrdersForRestaurant(isApproved, restaurantId);
        }

        [Authorize]
        [HttpGet("/users/{userEmail}")]
        public async Task<List<Order>> GetOrdersForRestaurants(string userEmail)
        {
            return await _orderService.GetAllOrdersForUser(userEmail);
        }
    }
}