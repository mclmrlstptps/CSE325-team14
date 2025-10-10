using MongoDB.Bson;
using MongoDB.Driver;
using RestaurantMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class MenuItemService
    {
        private readonly IMongoCollection<MenuItem> _menuItems;

        public MenuItemService(MongoDBService mongoDBService)
        {
            _menuItems = mongoDBService.MenuItems;
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync() =>
            await _menuItems.Find(_ => true).ToListAsync();

        public async Task<MenuItem?> GetMenuItemByIdAsync(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                var byOid = await _menuItems.Find(Builders<MenuItem>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
                if (byOid != null) return byOid;
            }

            var byStringId = await _menuItems.Find(Builders<MenuItem>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
            if (byStringId != null) return byStringId;

            var byProp = await _menuItems.Find(m => m.Id == id).FirstOrDefaultAsync();
            return byProp;
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem) =>
            await _menuItems.InsertOneAsync(menuItem);

        public async Task UpdateMenuItemAsync(string id, MenuItem menuItem)
        {
            var filter = BuildIdFilter(id);
            if (filter == null) return;

            var update = Builders<MenuItem>.Update
                .Set(m => m.Name, menuItem.Name)
                .Set(m => m.Description, menuItem.Description)
                .Set(m => m.Price, menuItem.Price)
                .Set(m => m.Reviews, menuItem.Reviews);

            await _menuItems.UpdateOneAsync(filter, update);
        }

        public async Task DeleteMenuItemAsync(string id)
        {
            var filter = BuildIdFilter(id);
            if (filter == null) return;
            await _menuItems.DeleteOneAsync(filter);
        }

        public async Task AddReviewAsync(string menuItemId, Review review)
        {
            var filter = BuildIdFilter(menuItemId);
            if (filter == null) return;
            var update = Builders<MenuItem>.Update.Push(m => m.Reviews, review);
            await _menuItems.UpdateOneAsync(filter, update);
        }

        public async Task<List<Review>> GetReviewsForMenuItemAsync(string id)
        {
            var menuItem = await GetMenuItemByIdAsync(id);
            return menuItem?.Reviews ?? new List<Review>();
        }

        private FilterDefinition<MenuItem>? BuildIdFilter(string id)
        {
            var defs = new List<FilterDefinition<MenuItem>>();

            if (ObjectId.TryParse(id, out var objectId))
            {
                defs.Add(Builders<MenuItem>.Filter.Eq("_id", objectId));
            }

            defs.Add(Builders<MenuItem>.Filter.Eq("_id", id));
            defs.Add(Builders<MenuItem>.Filter.Eq(m => m.Id, id));

            return Builders<MenuItem>.Filter.Or(defs);
        }
    }
}
