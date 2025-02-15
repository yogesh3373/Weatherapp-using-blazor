//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver;
//using ServerWeather.Data;
//using ServerWeather.Entity;

//namespace ServerWeather.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController(AppDbContext context) : ControllerBase
//    {
//        [HttpGet("/list")]
//        public async Task<IActionResult> Get()
//        {
//            var response = await context.WeatherForecast.Find(Builders<WeatherForecast>.Filter.Empty).ToListAsync();
//            return Ok(response);
//        }

//        [HttpGet("/list/G")]
//        public async Task<IActionResult> GetSingle(Guid id)
//        {
//            var entities = await context.WeatherForecast.Find(Builders<WeatherForecast>.Filter.Empty).ToListAsync();
//            var response = entities.FirstOrDefault(e => e.Id == id);
//            return Ok(response);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Post(WeatherForecast weather)
//        {
//            weather.Id = Guid.NewGuid();
//            await context.WeatherForecast.InsertOneAsync(weather);
//            return Ok(weather);
//        }
//    }
//}
