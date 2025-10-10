
using RestaurantMS.Models;

namespace RestaurantMS.Services
{
    public class MenuService
    {
        private readonly MongoDBService _mongoDBService;

        public MenuService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _mongoDBService.GetMenuItemsAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(string id)
        {
            return await _mongoDBService.GetMenuItemByIdAsync(id);
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {            
            await _mongoDBService.CreateMenuItemAsync(menuItem);
        }

        public async Task UpdateMenuItemAsync(string id, MenuItem menuItem)
        {
            await _mongoDBService.UpdateMenuItemAsync(id, menuItem);
        }

        public async Task DeleteMenuItemAsync(string id)
        {
            await _mongoDBService.DeleteMenuItemAsync(id);
        }
    }
}
