using Common.Dto;
using Common.KafkaEvents;
using Newtonsoft.Json;
using RestaurantService.Model;
using RestaurantService.Repository;

namespace RestaurantService.Services
{
    public interface IRestaurantService
    {
        //Restaurant owner
        Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO);
        Task<bool> CreateMenuItem(MenuItemDTO menuItemDTO, int restaurantId);
        Task<bool> UpdateMenuItem(int menuItemId, MenuItemDTO menuItemDTO);
        Task<bool> DeleteMenuItem(int menuItemId);

        //Customer
        Task<MenuDTO> GetRestaurantMenu(int restaurantId);
        Task<List<RestaurantDTO>> GetAllRestaurants();
        Task<MenuItemDTO> GetRestaurantMenuItem(int menuItemId);
        Task<bool> CheckMenuItemStock(CreateOrderDto createOrderDTO);
        Task<bool> UpdateMenuItemStock(CreateOrderDto createOrderDTO);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantProducerService _kafkaProducer;

        public RestaurantService(IRestaurantRepository restaurantRepository, IRestaurantProducerService kafkaProducer)
        {
            _restaurantRepository = restaurantRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<bool> CheckMenuItemStock(CreateOrderDto createOrderDTO)
        {
            var menuItemStock = await _restaurantRepository.CheckMenuItemStock(createOrderDTO.MenuItems);
            bool isInStock = menuItemStock.Any(x => x.StockCount < 1);
            return isInStock;
            //send message to hub if isInStock = false;
        }

        public async Task<bool> CreateMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            try
            {
                return await _restaurantRepository.CreateMenuItem(
                    new MenuItem
                        {Name = menuItemDTO.name, Price = menuItemDTO.price, Description = menuItemDTO.description},
                    restaurantId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO)
        {
            var cityInfo = new CityInfo {City = restaurantDTO.City, ZipCode = restaurantDTO.ZipCode};
            var address = new Address {StreetName = restaurantDTO.StreetName, CityInfo = cityInfo};
            return await _restaurantRepository.CreateRestaurant(new Restaurant
                {Name = restaurantDTO.RestaurantName, Address = address});
        }


        public async Task<bool> DeleteMenuItem(int menuItemId)
        {
            return await _restaurantRepository.DeleteMenuItem(menuItemId);
        }

        public Task<List<RestaurantDTO>> GetAllRestaurants()
        {
            throw new NotImplementedException();
        }

        public Task<MenuDTO> GetRestaurantMenu(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public Task<MenuItemDTO> GetRestaurantMenuItem(int MenuItemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMenuItem(int menuItemId, MenuItemDTO menuItemDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateMenuItemStock(CreateOrderDto createOrderDTO)
        {
            try
            {
                await _restaurantRepository.UpdateMenuItemStock(createOrderDTO.MenuItems);

                // Notify our order service..
                await _kafkaProducer.ProduceToKafka(EventStreamerEvents.SaveOrderEvent,
                    JsonConvert.SerializeObject(createOrderDTO));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}