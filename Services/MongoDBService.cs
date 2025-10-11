using MongoDB.Bson;
using MongoDB.Driver;
using RestaurantMS.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<MenuItem> _menuItems;

        public IMongoCollection<User> Users { get; }
        public IMongoCollection<MenuItem> MenuItems { get; }
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<Cart> Carts { get; }

        public MongoDBService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("MongoDB connection string is not configured.");

            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);
            settings.ConnectTimeout = TimeSpan.FromSeconds(30);
            settings.SocketTimeout = TimeSpan.FromSeconds(30);

            var client = new MongoClient(settings);
            _database = client.GetDatabase("RestaurantMS");
            _users = _database.GetCollection<User>("Users");
            _menuItems = _database.GetCollection<MenuItem>("MenuItems");

            Users = _database.GetCollection<User>("Users");
            MenuItems = _database.GetCollection<MenuItem>("MenuItems");
            Orders = _database.GetCollection<Order>("Orders");
            Carts = _database.GetCollection<Cart>("Carts");
        }

        public FilterDefinition<T> IdFilter<T>(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                throw new ArgumentException("Invalid ID format.");
            return Builders<T>.Filter.Eq("_id", objectId);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }

        // MenuItem operations
        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _menuItems.Find(menuItem => true).ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(string id)
        {
            return await _menuItems.Find(menuItem => menuItem.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {
            await _menuItems.InsertOneAsync(menuItem);
        }

        public async Task UpdateMenuItemAsync(string id, MenuItem menuItem)
        {
            await _menuItems.ReplaceOneAsync(mi => mi.Id == id, menuItem);
        }

        public async Task DeleteMenuItemAsync(string id)
        {
            await _menuItems.DeleteOneAsync(menuItem => menuItem.Id == id);
        }

        // to test connection
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB connection failed: {ex.Message}");
                return false;
            }
        }
    }
}
