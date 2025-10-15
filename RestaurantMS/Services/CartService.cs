using RestaurantMS.Models;

namespace RestaurantMS.Services
{
    public class CartService
    {
        public List<OrderItem> Cart { get; private set; } = new();

        public void AddToCart(MenuItem item, int quantity = 1)
        {
            var existing = Cart.FirstOrDefault(i => i.Name == item.Name);
            if (existing != null)
                existing.Quantity += quantity;
            else
                Cart.Add(new OrderItem
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = quantity
                });
        }

        public void IncreaseQuantity(OrderItem item) => item.Quantity++;
        public void DecreaseQuantity(OrderItem item)
        {
            if (item.Quantity > 1) item.Quantity--;
        }

        public void RemoveFromCart(OrderItem item) => Cart.Remove(item);

        public decimal GetTotal() => Cart.Sum(i => i.Price * i.Quantity);

        public void ClearCart() => Cart.Clear();
    }
}
