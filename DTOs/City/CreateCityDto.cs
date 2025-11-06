using System.ComponentModel.DataAnnotations;

namespace WeatherDashboardAPI.DTOs.City
{
    public class CreateCityDto
    {
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Country must be between 2 and 3 characters.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Latitude is required.")]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }

        [StringLength(10, ErrorMessage = "State code cannot exceed 10 characters.")]
        public string? StateCode { get; set; }
    }
}