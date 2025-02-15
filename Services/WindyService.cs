using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class WindyService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _mapForecastKey;
    private readonly string _pointForecastKey;

    public WindyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _mapForecastKey = _configuration["WindyApi:MapForecastKey"];
        _pointForecastKey = _configuration["WindyApi:PointForecastKey"];
    }

    public async Task<WeatherData> GetPointForecastAsync(double lat, double lon)
    {
        var request = new
        {
            lat = lat,
            lon = lon,
            model = "gfs",
            parameters = new[] { "wind", "temp", "precip", "clouds", "rh" },
            levels = new[] { "surface" },
            key = _pointForecastKey
        };

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", _pointForecastKey);

        var response = await _httpClient.PostAsJsonAsync("https://api.windy.com/api/point-forecast/v2", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WeatherData>();
    }

    public string GetMapForecastUrl(string layer, int zoom, int x, int y)
    {
        return $"https://tiles.windy.com/tiles/v9.0/{layer}/{zoom}/{x}/{y}.png?key={_mapForecastKey}";
    }

    public async Task<MapLayers> GetAvailableLayersAsync()
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", _mapForecastKey);

        var response = await _httpClient.GetAsync("https://api.windy.com/api/maps-forecast/v2/layers");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MapLayers>();
    }
}

public class WeatherData
{
    public double[] Temp { get; set; } = Array.Empty<double>();
    public double[] Wind { get; set; } = Array.Empty<double>();
    public double[] WindDir { get; set; } = Array.Empty<double>();
    public double[] Precip { get; set; } = Array.Empty<double>();
    public double[] Clouds { get; set; } = Array.Empty<double>();
    public double[] Rh { get; set; } = Array.Empty<double>(); // Relative humidity
}

public class MapLayers
{
    public List<string> Available { get; set; } = new();
    public Dictionary<string, LayerInfo> Info { get; set; } = new();
}

public class LayerInfo
{
    public string Name { get; set; }
    public string Unit { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
} 