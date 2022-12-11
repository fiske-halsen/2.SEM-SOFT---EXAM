using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Services;

namespace RestaurantService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
       
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        /// <summary>
        /// Creates a restaurant entity in the database for a restaurant owner
        /// </summary>
        /// <param name="restaurantDto"></param>
        /// <returns></returns>
        ///
        [Authorize]
        [HttpPost]
        public async Task<bool> CreateRestaurant([FromBody] RestaurantDTO restaurantDto)
        {
            return await _restaurantService.CreateRestaurant(restaurantDto);
        }
        /// <summary>
        /// Create a new menu-item for a given restaurants menu
        /// </summary>
        /// <param name="menuItemDto"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("/{restaurantId}/menu-item")]
        public async Task<bool> CreateMenuItem([FromBody] MenuItemDTO menuItemDto, int restaurantId)
        {
            return await _restaurantService.CreateMenuItem(menuItemDto, restaurantId);
        }
        /// <summary>
        /// updated a specific menu item from a restaurant menu
        /// </summary>
        /// <param name="menuItemDto"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("/{restaurantId}/menu-item")]
        public async Task<bool> UpdateMenuItem( [FromBody] MenuItemDTO menuItemDto, int restaurantId)
        {
            return await _restaurantService.UpdateMenuItem(menuItemDto, restaurantId);
        }
        /// <summary>
        /// Deletes a specific menu item from a restaurant menu
        /// </summary>
        /// <param name="menuItemDto"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("/{restaurantId}/menu-item")]
        public async Task<bool> DeleteMenuItem([FromBody] MenuItemDTO menuItemDto, int restaurantId)
        {
            return await _restaurantService.DeleteMenuItem(menuItemDto, restaurantId);
        }
        /// <summary>
        /// returns a menu from a specific restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/{restaurantId}/menu")]
        public async Task<MenuDTO> GetRestaurantMenu(int restaurantId)
        {
            return await _restaurantService.GetRestaurantMenu(restaurantId);
        }
        /// <summary>
        /// returns all restaurants
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<List<RestaurantDTO>> GetAllRestaurants()
        {
            return await _restaurantService.GetAllRestaurants();
        }
        /// <summary>
        /// returns a menu item from a specific restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuItemId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/{restaurantId}/menu-item/{menuItemId}")]
        public async Task<MenuItemDTO> GetRestaurantMenuItem(int restaurantId, int menuItemId)
        {
            return await _restaurantService.GetRestaurantMenuItem(restaurantId, menuItemId);
        }
        // Task<MenuItemDTO> GetRestaurantMenuItem(int menuItemId);
        

    }
}