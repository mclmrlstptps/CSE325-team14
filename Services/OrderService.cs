using MongoDB.Driver;
using RestaurantMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(MongoDBService mongoDBService)
        {
            _orders = mongoDBService.Orders;
        }

        public async Task<List<Order>> GetAllOrdersAsync() =>
            await _orders.Find(o => true).ToListAsync();

        public async Task<Order?> GetOrderByIdAsync(string id) =>
            await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task CreateOrderAsync(Order order) =>
            await _orders.InsertOneAsync(order);

        public async Task UpdateOrderAsync(string id, Order order) =>
            await _orders.ReplaceOneAsync(o => o.Id == id, order);

        public async Task DeleteOrderAsync(string id) =>
            await _orders.DeleteOneAsync(o => o.Id == id);

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId) =>
            await _orders.Find(o => o.UserId == userId).ToListAsync();
    }
}
