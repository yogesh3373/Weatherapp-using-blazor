namespace weather1.Models
{
    public class WeatherRecord
    {
        public string Name { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string email { get; set; }
    }

    public class GeolocationPosition
    {
        public GeolocationCoordinates Coords { get; set; } = new GeolocationCoordinates();
    }

    public class GeolocationCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}