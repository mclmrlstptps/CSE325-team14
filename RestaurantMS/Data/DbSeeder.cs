using MongoDB.Driver;
using RestaurantMS.Models;

namespace RestaurantMS.Data
{
    public static class DbSeeder
    {
        public static async Task SeedMenuItemsAsync(IMongoDatabase database)
        {
            var menuCollection = database.GetCollection<MenuItem>("MenuItems");

            var count = await menuCollection.CountDocumentsAsync(_ => true);
            if (count > 0) return;

            var sampleItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with fresh mozzarella, tomatoes, and basil.",
                    Price = 10.99M,
                    ImageUrl = "images/pizza-margherita.jpg",
                    Category = "Pizza"
                },
                new MenuItem
                {
                    Name = "Grilled Chicken Sandwich",
                    Description = "Juicy grilled chicken with lettuce, tomato, and mayo.",
                    Price = 8.49M,
                    ImageUrl = "images/chicken-sandwich.jpg",
                    Category = "Sandwich"
                },
                new MenuItem
                {
                    Name = "Caesar Salad",
                    Description = "Crisp romaine lettuce tossed with Caesar dressing and parmesan.",
                    Price = 7.25M,
                    ImageUrl = "images/caesar-salad.jpg",
                    Category = "Salad"
                },
                new MenuItem
                {
                    Name = "Beef Burger",
                    Description = "Grilled beef patty with cheese, onions, and special sauce.",
                    Price = 9.99M,
                    ImageUrl = "images/beef-burger.jpg",
                    Category = "Burger"
                }
            };

            await menuCollection.InsertManyAsync(sampleItems);
        }
    }
}
