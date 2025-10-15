using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantMS.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("menuItemId")]
        public string MenuItemId { get; set; } = string.Empty;

        [BsonElement("reviewerName")]
        public string ReviewerName { get; set; } = string.Empty;

        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;
    }
}
