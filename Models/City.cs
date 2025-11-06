namespace WeatherDashboardAPI.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? StateCode { get; set; }
        public int? OpenWeatherMapId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<WeatherRecord> WeatherRecords { get; set; } = new List<WeatherRecord>();
        public ICollection<UserFavoriteCity> FavoritedByUsers { get; set; } = new List<UserFavoriteCity>();
        public ICollection<WeatherAlert> WeatherAlerts { get; set; } = new List<WeatherAlert>();
    }
}