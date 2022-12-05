using Common.Dto;
using Common.ErrorModels;
using Common.KafkaEvents;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        //KAFKA
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
            if (menuItemStock == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Could not find stock for menu item");
            }
            bool isInStock = menuItemStock.Any(x => x.StockCount < 1);
            return isInStock;
            //send message to hub if isInStock = false;
        }

        public async Task<bool> CreateMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            return await _restaurantRepository.CreateMenuItem(
                new MenuItem
                    { Name = menuItemDTO.name, Price = menuItemDTO.price, Description = menuItemDTO.description },
                restaurantId);
        }

        public async Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO)
        {
            var cityInfo = new CityInfo { City = restaurantDTO.City, ZipCode = restaurantDTO.ZipCode };
            var address = new Address { StreetName = restaurantDTO.StreetName, CityInfo = cityInfo };
            return await _restaurantRepository.CreateRestaurant(new Restaurant
            {
                Name = restaurantDTO.RestaurantName, Address = address,
                Menu = new Menu
                {
                    MenuItems = restaurantDTO.Menu.MenuItems.Select(x => new MenuItem
                            { Description = x.description, Name = x.name, Price = x.price, StockCount = x.StockCount })
                        .ToList()
                }
            });
        }


        public async Task<bool> DeleteMenuItem(int menuItemId)
        {
            return await _restaurantRepository.DeleteMenuItem(menuItemId);
        }

        public async Task<List<RestaurantDTO>> GetAllRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllRestaurants();
            return restaurants.Select(x => new RestaurantDTO
            {
                City = x.Address.CityInfo.City, RestaurantName = x.Name, StreetName = x.Address.StreetName,
                ZipCode = x.Address.CityInfo.ZipCode
            }).ToList();
        }

        public async Task<MenuDTO> GetRestaurantMenu(int restaurantId)
        {
            var restaurantMenu = await _restaurantRepository.GetRestaurantMenu(restaurantId);
            return new MenuDTO
            {
                RestaurantName = restaurantMenu.Restaurant.Name,
                MenuItems = restaurantMenu.MenuItems.Select(x => new MenuItemDTO
                    { description = x.Description, name = x.Name, price = x.Price, Id = x.Id }).ToList()
            };
        }

        public async Task<MenuItemDTO> GetRestaurantMenuItem(int menuItemId)
        {
            var menuItem = await _restaurantRepository.GetRestaurantMenuItem(menuItemId);
            return new MenuItemDTO
                { Id = menuItem.Id, name = menuItem.Name, price = menuItem.Price, description = menuItem.Description };
        }

        public async Task<bool> UpdateMenuItem(int menuItemId, MenuItemDTO menuItemDTO)
        {
            return await _restaurantRepository.UpdateMenuItem(menuItemId, menuItemDTO);
        }

        public async Task<bool> UpdateMenuItemStock(CreateOrderDto createOrderDTO)
        {
            
                await _restaurantRepository.UpdateMenuItemStock(createOrderDTO.MenuItems);

                // Notify our order service..
                await _kafkaProducer.ProduceToKafka(EventStreamerEvents.SaveOrderEvent,
                    JsonConvert.SerializeObject(createOrderDTO));

                return true;
            
            
        }
    }
}