using MongoDB.Driver;
using ServerWeather.Entity;
using System.Collections.Generic;

namespace ServerWeather.Data
{
  public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration.GetConnectionString("Database");

            var Client = new MongoClient(connectionStrings);
            _database = Client.GetDatabase(databaseName);
        }

        public IMongoCollection<WeatherForecast> WeatherForecast =>
             _database.GetCollection<WeatherForecast>("WeatherForecast");

        public IMongoCollection<UserFavorites> FavoriteCity =>
            _database.GetCollection<UserFavorites>("FavoriteCity");

    }

}
