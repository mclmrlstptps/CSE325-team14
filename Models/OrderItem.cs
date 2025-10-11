using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantMS.Models
{
    public class OrderItem
    {
        // [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        // public string? Id { get; set; } // _id

        // [BsonElement("orderId")]
        // [BsonRepresentation(BsonType.ObjectId)]
        // public string? OrderId { get; set; }

        [BsonElement("menuItemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MenuItemId { get; set; }

        [BsonElement("menuItem")]
        public MenuItem? MenuItem { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; } = 1;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}