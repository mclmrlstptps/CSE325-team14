using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantMS.Models
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;
    }
}
