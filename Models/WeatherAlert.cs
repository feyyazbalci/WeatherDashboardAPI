namespace WeatherDashboardAPI.Models
{
    public class WeatherAlert
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public AlertType AlertType { get; set; }
        public string Message { get; set; } = string.Empty;
        public AlertSeverity Severity { get; set; }
        public double? TriggerValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property
        public City City { get; set; } = null!;
    }

    public enum AlertType
    {
        HighTemperature,
        LowTemperature,
        HeavyRain,
        HeavySnow,
        StrongWind,
        HighHumidity
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Danger
    }
}