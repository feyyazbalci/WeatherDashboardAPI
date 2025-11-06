namespace WeatherDashboardAPI.Models
{
    public class UserFavoriteCity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public City City { get; set; } = null!;
    }
}