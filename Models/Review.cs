using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RestaurantMS.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("reviewerName")]
        public string ReviewerName { get; set; } = string.Empty;

        [BsonElement("rating")]
        public int Rating { get; set; } // 1 to 5

        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
