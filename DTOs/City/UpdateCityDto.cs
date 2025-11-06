using System.ComponentModel.DataAnnotations;

namespace WeatherDashboardAPI.DTOs.City
{
    public class UpdateCityDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string? Country { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }

        [StringLength(10)]
        public string? StateCode { get; set; }
    }
}