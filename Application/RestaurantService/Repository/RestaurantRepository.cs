using Common.Dto;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Context;
using RestaurantService.Model;

namespace RestaurantService.Repository
{
    public interface IRestaurantRepository
    {
        Task<bool> CreateRestaurant(Restaurant restaurant);
        Task<bool> CreateMenuItem(MenuItem menuItem, int restaurantId);
        Task<bool> UpdateMenuItem(int menuItemId, MenuItemDTO menuItemDTO);
        Task<bool> DeleteMenuItem(int menuItemId);
        Task<List<MenuItem>> CheckMenuItemStock(List<MenuItemDTO> menuItems);
        Task<bool> UpdateMenuItemStock(List<MenuItemDTO> menuItems);

        //Customer
        Task<Menu> GetRestaurantMenu(int restaurantId);
        Task<List<Restaurant>> GetAllRestaurants();
        Task<MenuItem> GetRestaurantMenuItem(int MenuItemId);
    }

    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DBApplicationContext _dbContext;

        public RestaurantRepository(DBApplicationContext dBApplicationContext)
        {
            _dbContext = dBApplicationContext;
        }

        public async Task<List<MenuItem>> CheckMenuItemStock(List<MenuItemDTO> menuItems)
        {
            var menuItemIdList = menuItems.Select(x => x.Id).ToList();
            return await _dbContext.MenuItems.Where(x => menuItemIdList.Contains(x.Id)).ToListAsync();
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

        

        public async Task<bool> DeleteMenuItem(int menuItemId)
        {
            var menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == menuItemId);
            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _dbContext.Restaurants.ToListAsync();
        }

        public async Task<Menu> GetRestaurantMenu(int restaurantId)
        {
            return await _dbContext.Menus.Where(x => x.Restaurant.Id == restaurantId).FirstOrDefaultAsync();
        }

        public async Task<MenuItem> GetRestaurantMenuItem(int MenuItemId)
        {
            return await _dbContext.MenuItems.Where(x => x.Id == MenuItemId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMenuItem(int menuItemId, MenuItemDTO menuItemDTO)
        {
            var menuItemToUpdate = await _dbContext.MenuItems.FindAsync(menuItemId);
            menuItemToUpdate.Description = menuItemDTO.description;
            menuItemToUpdate.Name = menuItemDTO.name;
            menuItemToUpdate.Price = menuItemDTO.price;
            return true;
        }

        public async Task<bool> UpdateMenuItemStock(List<MenuItemDTO> menuItems)
        {
            var menuItemIdList = menuItems.Select(x => x.Id).ToList();
            var test = await _dbContext.MenuItems.Where(x => menuItemIdList.Contains(x.Id)).ToListAsync();
            test.ForEach(x => x.StockCount -= 1);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}