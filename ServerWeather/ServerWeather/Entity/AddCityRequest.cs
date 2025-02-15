using System.ComponentModel.DataAnnotations;

namespace ServerWeather.Entity
{
    public class AddCityRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }
    }
}
