namespace weather1.Models;

public class WeatherFoodRecommendation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string WeatherCondition { get; set; } = string.Empty;
    public List<FoodItem> RecommendedFoods { get; set; } = new();
    public double WaterIntake { get; set; }
    public List<string> HealthTips { get; set; } = new();
}

public class FoodItem
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Benefits { get; set; } = new();
    public string TimeOfDay { get; set; } = string.Empty;
} 