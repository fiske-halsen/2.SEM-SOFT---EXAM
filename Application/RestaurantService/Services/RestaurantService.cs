using Common.Dto;
using Common.ErrorModels;
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
        Task<bool> UpdateMenuItem(MenuItemDTO menuItemDTO, int restaurantId);
        Task<bool> DeleteMenuItem(MenuItemDTO menuItemDTO, int restaurantId);

        //Customer
        Task<MenuDTO> GetRestaurantMenu(int restaurantId);
        Task<List<RestaurantDTO>> GetAllRestaurants();

        Task<MenuItemDTO> GetRestaurantMenuItem(int restaurantId, int menuItemId);

        //KAFKA
        Task<bool> CheckMenuItemStock(CreateOrderDto createOrderDTO);
        Task<bool> UpdateMenuItemStock(CreateOrderDto createOrderDTO);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<bool> CheckMenuItemStock(CreateOrderDto createOrderDTO)
        {
            var menuItemStock =
                await _restaurantRepository.CheckMenuItemStockList(createOrderDTO.MenuItems.Select(_ => _.Id).ToList());

            if (menuItemStock == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Could not find stock for menu item");
            }

            bool isInStock = menuItemStock.Any(x => x.StockCount > 0);
            return isInStock;
            //send message to hub if isInStock = false;
        }

        public async Task<bool> CreateMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            return await _restaurantRepository.CreateMenuItem(
                new MenuItem
                    {Name = menuItemDTO.Name, Price = menuItemDTO.Price, Description = menuItemDTO.Description},
                restaurantId);
        }

        public async Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO)
        {
            var cityInfo = new CityInfo {City = restaurantDTO.City, ZipCode = restaurantDTO.ZipCode};
            var address = new Address {StreetName = restaurantDTO.StreetName, CityInfo = cityInfo};
            return await _restaurantRepository.CreateRestaurant(new Restaurant
            {
                Name = restaurantDTO.RestaurantName, Address = address,
                Menu = new Menu
                {
                    MenuItems = restaurantDTO.Menu.MenuItems.Select(x => new MenuItem
                            {Description = x.Description, Name = x.Name, Price = x.Price, StockCount = x.StockCount})
                        .ToList()
                }
            });
        }


        public async Task<bool> DeleteMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            return await _restaurantRepository.DeleteMenuItem(menuItemDTO.Id, restaurantId);
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
                    {Description = x.Description, Name = x.Name, Price = x.Price, Id = x.Id}).ToList()
            };
        }

        public async Task<MenuItemDTO> GetRestaurantMenuItem(int restaurantId, int menuItemId)
        {
            var menuItem = await _restaurantRepository.GetRestaurantMenuItem(restaurantId, menuItemId);
            return new MenuItemDTO
            {
                Id = menuItem.Id, Name = menuItem.Name, Price = menuItem.Price, Description = menuItem.Description,
                StockCount = menuItem.StockCount
            };
        }

        public async Task<bool> UpdateMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            return await _restaurantRepository.UpdateMenuItem(menuItemDTO, restaurantId);
        }

        public async Task<bool> UpdateMenuItemStock(CreateOrderDto createOrderDTO)
        {
            await _restaurantRepository.UpdateMenuItemStock(createOrderDTO.MenuItems.Select(_ => _.Id).ToList());


            return true;
        }
    }
}