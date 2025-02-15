using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServerWeather.Data;
using ServerWeather.Entity;

namespace ServerWeather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserFavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserFavoritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var response = await _context.FavoriteCity.Find(Builders<UserFavorites>.Filter.Empty).ToListAsync();
            return Ok(response);
        }

        [HttpGet("/favorites/{email}")]
        public async Task<IActionResult> GetFavoritesByEmail(string email)
        {
            var response = await _context.FavoriteCity.Find(x => x.Email == email).FirstOrDefaultAsync();
            if (response == null) return NotFound("No favorites found for this email");
            return Ok(response);
        }

        [HttpPost("/favorites/add")]
        public async Task<IActionResult> AddFavoriteCity([FromBody] AddCityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filter = Builders<UserFavorites>.Filter.Eq(x => x.Email, request.Email);
            var user = await _context.FavoriteCity.Find(filter).FirstOrDefaultAsync();

            if (user == null)
            {
                user = new UserFavorites
                {
                    Email = request.Email,
                    FavoriteCities = new List<string> { request.City },
                    CreatedAt = DateTime.UtcNow
                };
                await _context.FavoriteCity.InsertOneAsync(user);
            }
            else
            {
                if (!user.FavoriteCities.Contains(request.City))
                {
                    user.FavoriteCities.Add(request.City);
                    var update = Builders<UserFavorites>.Update
                        .Set(x => x.FavoriteCities, user.FavoriteCities)
                        .CurrentDate(x => x.CreatedAt);
                    await _context.FavoriteCity.UpdateOneAsync(filter, update);
                }
            }

            return Ok(user);
        }

        [HttpPost("/favorites/remove")]
        public async Task<IActionResult> RemoveFavoriteCity([FromBody] RemoveCityRequest request)
        {
            var filter = Builders<UserFavorites>.Filter.Eq(x => x.Email, request.Email);
            var user = await _context.FavoriteCity.Find(filter).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("No user found with this email.");
            }

            if (user.FavoriteCities.Remove(request.City))
            {
                var update = Builders<UserFavorites>.Update
                    .Set(x => x.FavoriteCities, user.FavoriteCities)
                    .CurrentDate(x => x.CreatedAt);

                await _context.FavoriteCity.UpdateOneAsync(filter, update);
            }

            return Ok(user);
        }

        public class RemoveCityRequest
        {
            public string Email { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
        }

    }
}
