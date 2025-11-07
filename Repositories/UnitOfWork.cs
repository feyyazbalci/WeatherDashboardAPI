using WeatherDashboardAPI.Data;
using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<User> Users { get; private set; }
        public IRepository<City> Cities { get; private set; }
        public IRepository<WeatherRecord> WeatherRecords { get; private set; }
        public IRepository<UserFavoriteCity> UserFavoriteCities { get; private set; }
        public IRepository<WeatherAlert> WeatherAlerts { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Users = new Repository<User>(context);
            Cities = new Repository<City>(context);
            WeatherRecords = new Repository<WeatherRecord>(context);
            UserFavoriteCities = new Repository<UserFavoriteCity>(context);
            WeatherAlerts = new Repository<WeatherAlert>(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}