using MongoDB.Bson;
using MongoDB.Driver;
using RestaurantMS.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<MenuItem> _menuItems;

        public MongoDBService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");

            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);
            settings.ConnectTimeout = TimeSpan.FromSeconds(30);
            settings.SocketTimeout = TimeSpan.FromSeconds(30);

            var client = new MongoClient(settings);
            _database = client.GetDatabase("RestaurantMS");

            _users = _database.GetCollection<User>("Users");
            _menuItems = _database.GetCollection<MenuItem>("MenuItems");
        }

        // Helper filter for MenuItem IDs
        private FilterDefinition<MenuItem> MenuItemIdFilter(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                throw new ArgumentException("Invalid menu item ID format.");

            return Builders<MenuItem>.Filter.Eq("_id", objectId);
        }

        // Helper filter for User IDs
        private FilterDefinition<User> UserIdFilter(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                throw new ArgumentException("Invalid user ID format.");

            return Builders<User>.Filter.Eq("_id", objectId);
        }

        // ---------------- User operations ----------------
        public async Task<List<User>> GetUsersAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _users.Find(UserIdFilter(id)).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            var filter = UserIdFilter(id);
            await _users.ReplaceOneAsync(filter, user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _users.DeleteOneAsync(UserIdFilter(id));
        }

        // ---------------- MenuItem operations ----------------
        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _menuItems.Find(item => true).ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(string id)
        {
            return await _menuItems.Find(MenuItemIdFilter(id)).FirstOrDefaultAsync();
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {
            await _menuItems.InsertOneAsync(menuItem);
        }

        public async Task UpdateMenuItemAsync(string id, MenuItem menuItem)
        {
            menuItem.Id = id;
            await _menuItems.ReplaceOneAsync(MenuItemIdFilter(id), menuItem);
        }

        public async Task DeleteMenuItemAsync(string id)
        {
            await _menuItems.DeleteOneAsync(MenuItemIdFilter(id));
        }

        // ---------------- Review operations ----------------
        public async Task AddReviewToMenuItemAsync(string menuItemId, Review review)
        {
            var update = Builders<MenuItem>.Update.Push(m => m.Reviews, review);
            await _menuItems.UpdateOneAsync(MenuItemIdFilter(menuItemId), update);
        }

        // ---------------- Test connection ----------------
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await _database.RunCommandAsync((Command<object>)"{ping:1}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
