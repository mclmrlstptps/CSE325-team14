using MongoDB.Driver;
using RestaurantMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartService(MongoDBService mongoDBService)
        {
            _carts = mongoDBService.Carts;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId) =>
            await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();

        public async Task AddItemToCartAsync(string userId, OrderItem orderItem)
        {
            var cart = await GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<OrderItem> { orderItem },
                    Total = orderItem.Price * orderItem.Quantity
                };
                await _carts.InsertOneAsync(cart);
            }
            else
            {
                cart.Items.Add(orderItem);
                cart.Total += orderItem.Price * orderItem.Quantity;
                await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }
        }

        public async Task ClearCartAsync(string userId) =>
            await _carts.DeleteOneAsync(c => c.UserId == userId);

        public async Task<decimal> GetTotalAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            return cart?.Total ?? 0;
        }
    }
}
