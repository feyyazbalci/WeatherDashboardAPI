namespace WeatherDashboardAPI.DTOs.Favorite
{
    public class FavoriteCityDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }
}