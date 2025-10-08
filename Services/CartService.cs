using System.Collections.Generic;
using System.Linq;
using RestaurantMS.Data;
using RestaurantMS.Models;


namespace RestaurantMS.Services
{
    // Manages cart state for the customer
    public class CartService
    {
        // List of items currently in the cart
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Add item to the cart or increase quantity if already exists
        public void AddItem(MenuItem menuItem, int qty = 1)
        {
            var existing = Items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
            if (existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                Items.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    MenuItem = menuItem,
                    Quantity = qty,
                    Price = menuItem.Price
                });
            }
        }

        // Update quantity of an item or remove if quantity <= 0
        public void UpdateQuantity(int menuItemId, int qty)
        {
            var item = Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                item.Quantity = qty;
                if (item.Quantity <= 0)
                    Items.Remove(item);
            }
        }

        // Calculate total price
        public decimal GetTotal() => Items.Sum(i => i.Quantity * i.Price);

        // Clear cart after checkout
        public void Clear() => Items.Clear();
    }
}
