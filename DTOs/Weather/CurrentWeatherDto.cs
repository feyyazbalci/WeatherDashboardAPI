namespace WeatherDashboardAPI.DTOs.Weather
{
    public class CurrentWeatherDto
    {
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public string WeatherDescription { get; set; } = string.Empty;
        public string? WeatherIcon { get; set; }
        public double WindSpeed { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}