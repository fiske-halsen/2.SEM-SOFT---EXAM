using Common.Dto;
using RestaurantService.Model;
using RestaurantService.Repository;
using System.Collections.Generic;

namespace RestaurantService.Services
{
    public interface IRestaurantService
    {
        //restaurant owner
        Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO);
        Task<bool> CreateMenuItem(MenuItemDTO menuItemDTO, int restaurantId);
        Task<bool> UpdateMenuItem (int menuItemId, MenuItemDTO menuItemDTO);
        Task <bool> DeleteMenuItem (int menuItemId);

        //Customer
        Task<MenuDTO> GetRestaurantMenu(int restaurantId);
        Task <List<RestaurantDTO>>GetAllRestaurants();
        Task<MenuItemDTO> GetRestaurantMenuItem(int menuItemId);
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
                new MenuItem { Name = menuItemDTO.name, Price = menuItemDTO.price, Description = menuItemDTO.description }, restaurantId);
                 
            }
            catch (Exception)
            {

                throw;
            }
         }
            

        public async Task<bool> CreateRestaurant(RestaurantDTO restaurantDTO)
        {
            var cityInfo = new CityInfo { City = restaurantDTO.City, ZipCode = restaurantDTO.ZipCode };
            var address = new Address { StreetName = restaurantDTO.StreetName, CityInfo = cityInfo };
            return await _restaurantRepository.CreateRestaurant(new Restaurant { Name = restaurantDTO.RestaurantName, Address = address });
             
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
            
            return await _restaurantRepository.UpdateMenuItemStock(createOrderDTO.MenuItems);
        }
    }
}
