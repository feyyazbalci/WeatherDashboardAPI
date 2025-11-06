namespace WeatherDashboardAPI.Models
{
    public class WeatherRecord
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public int? WindDegree { get; set; }
        public int? Cloudiness { get; set; }
        public double? Rainfall {get; set; }
        public double? Snowfall { get; set; }

        public string WeatherMain { get; set; } = string.Empty; 
        public string WeatherDescription { get; set; } = string.Empty; 
        public string? WeatherIcon { get; set; } 

        public DateTime RecordedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public City City { get; set; } = null!;

    }
}