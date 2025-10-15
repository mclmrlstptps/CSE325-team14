using MongoDB.Driver;
using RestaurantMS.Data;
using RestaurantMS.Models;

namespace RestaurantMS.Services
{
    public class OrderService
    {
        private readonly MongoDbContext _context;

        public OrderService(MongoDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.InsertOneAsync(order);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _context.Orders.Find(o => o.UserId == userId)
                                        .SortByDescending(o => o.CreatedAt)
                                        .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Find(_ => true)
                                        .SortByDescending(o => o.CreatedAt)
                                        .ToListAsync();
        }
    }
}
