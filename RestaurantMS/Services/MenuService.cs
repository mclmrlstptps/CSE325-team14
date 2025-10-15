using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Models;

namespace RestaurantMS.Services
{
    public class MenuService
    {
        private readonly IMongoCollection<MenuItem> _menuCollection;

        public MenuService(MongoDbContext context)
        {
            _menuCollection = context.Database.GetCollection<MenuItem>("MenuItems");
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _menuCollection.Find(_ => true).ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(string id)
        {
            return await _menuCollection.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddMenuItemAsync(MenuItem newItem)
        {
            await _menuCollection.InsertOneAsync(newItem);
        }

        public async Task UpdateMenuItemAsync(string id, MenuItem updatedItem)
        {
            await _menuCollection.ReplaceOneAsync(item => item.Id == id, updatedItem);
        }

        public async Task DeleteMenuItemAsync(string id)
        {
            await _menuCollection.DeleteOneAsync(item => item.Id == id);
        }
    }
}
