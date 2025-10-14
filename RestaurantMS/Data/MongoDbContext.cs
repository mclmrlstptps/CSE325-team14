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

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
