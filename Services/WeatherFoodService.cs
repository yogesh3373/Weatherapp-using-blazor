using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
 using weather1.Models;
using Supabase;
using Microsoft.AspNetCore.Components;

namespace weather1.Services
{
    public interface IWeatherFoodService
    {
        Task<WeatherFoodRecommendation> GetRecommendations(double temperature, int humidity, string condition);
    }

    public class WeatherFoodService : IWeatherFoodService
    {
        private readonly Client _supabaseClient;
        private readonly NavigationManager _navigationManager;

        public WeatherFoodService(Client supabaseClient, NavigationManager navigationManager)
        {
            _supabaseClient = supabaseClient;
            _navigationManager = navigationManager;
        }

        public async Task<WeatherFoodRecommendation> GetRecommendations(double temperature, int humidity, string condition)
        {
            var recommendation = new WeatherFoodRecommendation
            {
                Temperature = temperature,
                Humidity = humidity,
                WeatherCondition = condition,
                WaterIntake = CalculateWaterIntake(temperature, humidity)
            };

            recommendation.RecommendedFoods = await GetFoodRecommendations(temperature);
            recommendation.HealthTips = GenerateHealthTips(temperature, humidity);

            return recommendation;
        }

        private double CalculateWaterIntake(double temperature, int humidity)
        {
            double baseIntake = 2.5;
            if (temperature > 25) baseIntake += 0.5;
            if (humidity < 40) baseIntake += 0.2;
            return Math.Round(baseIntake, 1);
        }

        private async Task<List<FoodItem>> GetFoodRecommendations(double temperature)
        {
            var foods = new List<FoodItem>();
            if (temperature > 30)
            {
                foods.AddRange(GetHotWeatherFoods());
            }
            else if (temperature > 20)
            {
                foods.AddRange(GetModerateWeatherFoods());
            }
            else
            {
                foods.AddRange(GetCoolWeatherFoods());
            }
            return foods;
        }

        private List<FoodItem> GetHotWeatherFoods()
        {
            return new List<FoodItem>
            {
                new() {
                    Name = "Coconut Water",
                    Category = "Beverages",
                    Benefits = new() { "Natural electrolytes", "Cooling effect" },
                    TimeOfDay = "Morning"
                },
                new() {
                    Name = "Watermelon",
                    Category = "Fruits",
                    Benefits = new() { "High water content", "Natural coolant" },
                    TimeOfDay = "Afternoon"
                }
            };
        }

        private List<FoodItem> GetModerateWeatherFoods()
        {
            return new List<FoodItem>
            {
                new() {
                    Name = "Mixed Fruit Bowl",
                    Category = "Fruits",
                    Benefits = new() { "Balanced nutrition", "Natural energy" },
                    TimeOfDay = "Morning"
                }
            };
        }

        private List<FoodItem> GetCoolWeatherFoods()
        {
            return new List<FoodItem>
            {
                new() {
                    Name = "Hot Green Tea",
                    Category = "Beverages",
                    Benefits = new() { "Warming effect", "Antioxidants" },
                    TimeOfDay = "Morning"
                }
            };
        }

        private List<string> GenerateHealthTips(double temperature, int humidity)
        {
            return new List<string>
            {
                $"Recommended water intake: {CalculateWaterIntake(temperature, humidity)} liters",
                "Drink water every 2 hours",
                "Listen to your body's thirst signals"
            };
        }
    }
}

public interface IGeolocationService
{
    Task<(double latitude, double longitude)> GetCurrentPosition();
}

public class GeolocationService : IGeolocationService
{
    private readonly IJSRuntime _jsRuntime;

    public GeolocationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<(double latitude, double longitude)> GetCurrentPosition()
    {
        try
        {
            var position = await _jsRuntime.InvokeAsync<GeolocationPosition>("getCurrentPosition");
            return (position.Coords.Latitude, position.Coords.Longitude);
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting geolocation", ex);
        }
    }
}

public interface IWeatherService
{
    Task<WeatherRecord> GetWeatherByCoordinates(double latitude, double longitude);
    Task<WeatherRecord> GetWeatherByLocation(string location);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "your-weather-api-key";

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherRecord> GetWeatherByCoordinates(double latitude, double longitude)
    {
        // Implement weather API call using coordinates
        throw new NotImplementedException();
    }

    public async Task<WeatherRecord> GetWeatherByLocation(string location)
    {
        // Implement weather API call using location name
        throw new NotImplementedException();
    }
} 