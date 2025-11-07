using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<City> Cities { get; }
        IRepository<WeatherRecord> WeatherRecords { get; }
        IRepository<UserFavoriteCity> UserFavoriteCities { get; }
        IRepository<WeatherAlert> WeatherAlerts { get; }

        Task<int> SaveChangesAsync();

    }
}