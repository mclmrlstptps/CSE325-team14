using MongoDB.Driver;
using RestaurantMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDBService mongoDBService)
        {
            _users = mongoDBService.Users;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _users.Find(u => true).ToListAsync();

        public async Task<User?> GetUserByIdAsync(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _users.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateUserAsync(string id, User user) =>
            await _users.ReplaceOneAsync(u => u.Id == id, user);

        public async Task DeleteUserAsync(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);
    }
}
