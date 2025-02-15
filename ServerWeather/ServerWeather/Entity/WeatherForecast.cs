using MongoDB.Bson.Serialization.Attributes;

namespace ServerWeather.Entity
{
    public class WeatherForecast
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("Id")]
        public Guid Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Temperature")]
        public int TemperatureC { get; set; }
        [BsonElement("Summary")]
        public string Summary { get; set; }

    }
}
