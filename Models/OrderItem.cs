using System;

namespace RestaurantMS.Models
{
    // Represents a single menu item in an order
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        // MenuItem being ordered
        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        // Quantity of this item
        public int Quantity { get; set; } = 1;

        // Price at the time of ordering
        public decimal Price { get; set; }
    }
}
