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
        /// <summary>
        /// creates a menu item for a restaurants menu in the database
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<bool> CreateMenuItem(MenuItem menuItem, int restaurantId)
        {
            var menu = await _dbContext.Menus.Where(x => x.Restaurant.Id == restaurantId).Include(c => c.Restaurant).FirstOrDefaultAsync();
            menu.MenuItems.Add(menuItem);
            await _dbContext.MenuItems.AddAsync(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// creates a restayurant to the database
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        public async Task<bool> CreateRestaurant(Restaurant restaurant)
        {
            await _dbContext.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// deletes a menu item from the restaurant database
        /// </summary>
        /// <param name="menuItemId"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMenuItem(int menuItemId, int restaurantId)
        {
            var menuItem =
                await _dbContext.MenuItems.FirstOrDefaultAsync(x =>
                    x.Id == menuItemId && x.Menu.Restaurant.Id == restaurantId);
            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// gets all the restaurants from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _dbContext.Restaurants.Include(x => x.Menu).Include(c => c.Address)
                .Include(b => b.Address.CityInfo).ToListAsync();
        }
        /// <summary>
        /// gets a specific restaurant menu from the database by a int id
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<Menu> GetRestaurantMenu(int restaurantId)
        {
            var menu = await _dbContext.Menus.Where(x => x.Restaurant.Id == restaurantId).Include(c => c.Restaurant).Include(v => v.MenuItems)
               
                .FirstOrDefaultAsync();

            return menu;
        }
        /// <summary>
        /// gets a specific menu item from the restaurant database
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="MenuItemId"></param>
        /// <returns></returns>
        public async Task<MenuItem> GetRestaurantMenuItem(int restaurantId, int MenuItemId)
        {
            return await _dbContext.MenuItems.Where(x => x.Id == MenuItemId && x.Menu.Restaurant.Id == restaurantId)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// updates information about a menu item to the database
        /// </summary>
        /// <param name="menuItemDTO"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// updates the stock on a menu item for a restaurants menu. Should probobly be refactored to the service layer instead
        /// </summary>
        /// <param name="menuItemsIds"></param>
        /// <returns></returns>
        public async Task<bool> UpdateMenuItemStock(List<int> menuItemsIds)
        {
            var test = await _dbContext.MenuItems.Where(x => menuItemsIds.Contains(x.Id)).ToListAsync();
            test.ForEach(x => x.StockCount -= 1);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}