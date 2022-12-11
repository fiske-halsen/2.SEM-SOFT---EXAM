using Common.Dto;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IRestaurantServiceCommunicator
    {
        Task<bool> CreateRestaurant(CreateRestaurantDto restaurantDto);
        Task<bool> CreateMenuItem(CreateMenuItemDto menuItemDto, int restaurantId);
        Task<bool> UpdateMenuItem(MenuItemDTO menuItemDto, int restaurantId);
        Task<bool> DeleteMenuItem(int menuItemId, int restaurantId);
        Task<MenuDTO> GetRestaurantMenu(int restaurantId);
        Task<IEnumerable<RestaurantDTO>> GetAllRestaurants();
        Task<MenuItemDTO> GetMenuItem(int restaurantId, int menuItemId);
    }

    public class RestaurantServiceCommunicator : IRestaurantServiceCommunicator
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HelperService _helperService;
        private readonly ApplicationCredentials _applicationCredentials;
        private string _restaurantServiceUrl;

        public RestaurantServiceCommunicator(
            IApiService apiService,
            ITokenService tokenService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            HelperService helperService)

        {
            _apiService = apiService;
            _tokenService = tokenService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _helperService = helperService;
            _restaurantServiceUrl = configuration["RestaurantService:Host"];
            _applicationCredentials =
                _helperService.GetMicroServiceApplicationCredentials(HelperService.ClientType.RestaurantService);
        }

        /// <summary>
        /// Sends a http call to restaurant service to create a menu item to a restaurant
        /// </summary>
        /// <param name="menuItemDto"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateMenuItem(CreateMenuItemDto menuItemDto, int restaurantId)
        {
            return await _apiService.Post($"{_restaurantServiceUrl}/{restaurantId}/menu-item",
                JsonConvert.SerializeObject(menuItemDto), _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to create a new restaurant
        /// </summary>
        /// <param name="restaurantDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateRestaurant(CreateRestaurantDto restaurantDto)
        {
            return await _apiService.Post($"{_restaurantServiceUrl}/Restaurant",
                JsonConvert.SerializeObject(restaurantDto), _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to update a menu item for a restaurant
        /// </summary>
        /// <param name="menuItemDto"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateMenuItem(MenuItemDTO menuItemDto, int restaurantId)
        {
            return await _apiService.Put($"{_restaurantServiceUrl}/{restaurantId}/menu-item",
                JsonConvert.SerializeObject(menuItemDto), _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to delete a menu item for a restaurant
        /// </summary>
        /// <param name="menuItemId"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteMenuItem(int menuItemId, int restaurantId)
        {
            return await _apiService.Delete($"{_restaurantServiceUrl}/{restaurantId}/menu-item/{menuItemId}",
                _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to get a restaurant menu
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MenuDTO> GetRestaurantMenu(int restaurantId)
        {
            return await _apiService.GetSingle<MenuDTO>($"{_restaurantServiceUrl}/{restaurantId}/menu",
                _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to get all restaurants
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<RestaurantDTO>> GetAllRestaurants()
        {
            return await _apiService.Get<RestaurantDTO>($"{_restaurantServiceUrl}/Restaurant", _applicationCredentials);
        }

        /// <summary>
        /// Sends a http call to restaurant service to get a specific menu item for a restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuItemId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MenuItemDTO> GetMenuItem(int restaurantId, int menuItemId)
        {
            return await _apiService.GetSingle<MenuItemDTO>(
                $"{_restaurantServiceUrl}/{restaurantId}/menu-item/{menuItemId}", _applicationCredentials);
        }
    }
}