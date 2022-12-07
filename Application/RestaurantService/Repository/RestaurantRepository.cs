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

        Task<List<MenuItem>> CheckMenuItemStockList(List<int> menuItemsIds);
        Task<bool> UpdateMenuItemStock(List<int> menuItemsIds);

        Task<bool> UpdateMenuItem(MenuItemDTO menuItemDTO, int restaurantId);
        Task<bool> DeleteMenuItem(int menuItemId, int restaurantId);

        //Customer
        Task<Menu> GetRestaurantMenu(int restaurantId);
        Task<List<Restaurant>> GetAllRestaurants();
        Task<MenuItem> GetRestaurantMenuItem(int restaurantId, int MenuItemId);
    }

    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DBApplicationContext _dbContext;

        public RestaurantRepository(DBApplicationContext dBApplicationContext)
        {
            _dbContext = dBApplicationContext;
        }

        public async Task<List<MenuItem>> CheckMenuItemStockList(List<int> menuItemsIds)
        {
            return await _dbContext.MenuItems.Where(x => menuItemsIds.Contains(x.Id)).ToListAsync();
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


        public async Task<bool> DeleteMenuItem(int menuItemId, int restaurantId)
        {
            var menuItem =
                await _dbContext.MenuItems.FirstOrDefaultAsync(x =>
                    x.Id == menuItemId && x.Menu.Restaurant.Id == restaurantId);
            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _dbContext.Restaurants.Include(x => x.Menu).Include(c => c.Address)
                .Include(b => b.Address.CityInfo).ToListAsync();
        }

        public async Task<Menu> GetRestaurantMenu(int restaurantId)
        {
            var restaurant = await _dbContext.Restaurants.Where(x => x.Id == restaurantId)
                .Include(c => c.Menu.MenuItems)
                .FirstOrDefaultAsync();

            return restaurant.Menu;
        }

        public async Task<MenuItem> GetRestaurantMenuItem(int restaurantId, int MenuItemId)
        {
            return await _dbContext.MenuItems.Where(x => x.Id == MenuItemId && x.Menu.Restaurant.Id == restaurantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMenuItem(MenuItemDTO menuItemDTO, int restaurantId)
        {
            var menuItemToUpdate = await _dbContext.MenuItems
                .Where(x => x.Menu.Restaurant.Id == restaurantId && x.Id == menuItemDTO.Id)
                .FirstOrDefaultAsync();

            menuItemToUpdate.Description = menuItemDTO.Description;
            menuItemToUpdate.Name = menuItemDTO.Name;
            menuItemToUpdate.Price = menuItemDTO.Price;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMenuItemStock(List<int> menuItemsIds)
        {
            var test = await _dbContext.MenuItems.Where(x => menuItemsIds.Contains(x.Id)).ToListAsync();
            test.ForEach(x => x.StockCount -= 1);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}