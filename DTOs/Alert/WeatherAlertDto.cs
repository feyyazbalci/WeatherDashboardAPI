using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.DTOs.Alert
{
    public class WeatherAlertDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public double? TriggerValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}