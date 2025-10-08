using System;
using System.Collections.Generic;

namespace RestaurantMS.Models
{
    // Represents a customer's order
    public class Order
    {
        public int Id { get; set; }

        // List of items in the order
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Total price of the order
        public decimal Total { get; set; }

        // Current status of the order
        public string Status { get; set; } = "Pending";

        // Timestamp when the order was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Customer information (guest or authenticated)
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        // Optional: link to authenticated user
        public string? UserId { get; set; }
        // public virtual ApplicationUser? User { get; set; } // Uncomment if using Identity
    }
}
