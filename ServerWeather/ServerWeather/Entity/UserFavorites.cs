using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerWeather.Entity
{
    public class UserFavorites
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("FavoriteCities")]
        public List<string> FavoriteCities { get; set; } = new List<string>();

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
