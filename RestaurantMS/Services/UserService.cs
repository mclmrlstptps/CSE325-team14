using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Models;

namespace RestaurantMS.Services
{
    public class UserService
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserService(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(ApplicationUser user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}
