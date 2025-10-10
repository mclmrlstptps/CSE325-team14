using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantMS.Models
{
    public class Review
    {
        [BsonElement("reviewerName")]
        public string ReviewerName { get; set; } = string.Empty;

        [BsonElement("rating")]
        public int Rating { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;
    }
}
