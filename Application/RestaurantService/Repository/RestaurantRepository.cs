using Microsoft.EntityFrameworkCore;
using RestaurantService.Context;
using RestaurantService.Model;

namespace RestaurantService.Repository
{
    public interface IRestaurantRepository
    {
        

        Task<bool> CreateRestaurant(Restaurant restaurant);
        Task<bool> CreateRestaurantMenu(Menu menu);
        Task<bool> CreateMenuItem(MenuItem menuItem, int restaurantId);
        Task<bool> UpdateMenuItem(MenuItem menuItem, MenuItemDTO menuItemDTO);
        Task<bool> DeleteMenuItem(int menuItemId);

        //Customer
        Task<MenuDTO> GetRestaurantMenu(int restaurantId);
        Task<List<RestaurantDTO>> GetAllRestaurants();
        Task<MenuItemDTO> GetRestaurantMenuItem(int MenuItemId);



    }
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DBApplicationContext _dbContext;

        public RestaurantRepository(DBApplicationContext dBApplicationContext)
        {
            _dbContext = dBApplicationContext;
        }

        

        public async Task<bool> CreateMenuItem(MenuItem menuItem, int restaurantId)
        {
            var menu = await _dbContext.Menus.Where(x => x.Restaurant.Id == restaurantId).FirstOrDefaultAsync();
            menu.MenuItems.Add(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateRestaurant(Restaurant restaurant)
        {
            await _dbContext.AddAsync(restaurant);
            return true;
        }

        public Task<bool> CreateRestaurantMenu(Menu menu)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteMenuItem(int menuItemId)
        {
            var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == menuItemId);
             _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
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

        public Task<bool> UpdateMenuItem(MenuItem menuItem, MenuItemDTO menuItemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
