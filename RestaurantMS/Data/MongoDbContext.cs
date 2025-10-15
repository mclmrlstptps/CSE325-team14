using MongoDB.Driver;
using RestaurantMS.Models;

namespace RestaurantMS.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client)
        {
            _database = client.GetDatabase("RestaurantMS");
        }
        
        public IMongoCollection<MenuItem> MenuItems => _database.GetCollection<MenuItem>("MenuItems");
        public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("Users");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoDatabase Database => _database;
    }
}
