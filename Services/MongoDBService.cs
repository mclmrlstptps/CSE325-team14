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
