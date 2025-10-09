using System.Threading.Tasks;
using RestaurantMS.Data;
using RestaurantMS.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantMS.Services
{
    // Handles order persistence
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Save order to database
        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
