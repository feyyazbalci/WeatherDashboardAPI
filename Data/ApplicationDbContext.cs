using Microsoft.EntityFrameworkCore;
using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
        public DbSet<UserFavoriteCity> UserFavoriteCities { get; set; }
        public DbSet<WeatherAlert> WeatherAlerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            // City configuration
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Name, e.Country });
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(2); // ISO country code
            });

            // WeatherRecord configuration
            modelBuilder.Entity<WeatherRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RecordedAt);
                entity.HasIndex(e => new { e.CityId, e.RecordedAt });
                
                entity.HasOne(e => e.City)
                    .WithMany(c => c.WeatherRecords)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserFavoriteCity configuration
            modelBuilder.Entity<UserFavoriteCity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.CityId }).IsUnique();
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.FavoriteCities)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.City)
                    .WithMany(c => c.FavoritedByUsers)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WeatherAlert>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.IsActive);

                entity.HasOne(e => e.City)
                    .WithMany(c => c.WeatherAlerts)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            var seedDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<City>().HasData(
                new City 
                { 
                    Id = 1, 
                    Name = "Istanbul", 
                    Country = "TR", 
                    Latitude = 41.0082, 
                    Longitude = 28.9784,
                    OpenWeatherMapId = 745044,
                    CreatedAt = seedDate
                },
                new City 
                { 
                    Id = 2, 
                    Name = "Ankara", 
                    Country = "TR", 
                    Latitude = 39.9334, 
                    Longitude = 32.8597,
                    OpenWeatherMapId = 323786,
                    CreatedAt = seedDate
                },
                new City 
                { 
                    Id = 3, 
                    Name = "Izmir", 
                    Country = "TR", 
                    Latitude = 38.4237, 
                    Longitude = 27.1428,
                    OpenWeatherMapId = 311046,
                    CreatedAt = seedDate
                },
                new City 
                { 
                    Id = 4, 
                    Name = "London", 
                    Country = "GB", 
                    Latitude = 51.5074, 
                    Longitude = -0.1278,
                    OpenWeatherMapId = 2643743,
                    CreatedAt = seedDate
                },
                new City 
                { 
                    Id = 5, 
                    Name = "New York", 
                    Country = "US", 
                    Latitude = 40.7128, 
                    Longitude = -74.0060,
                    OpenWeatherMapId = 5128581,
                    CreatedAt = seedDate
                }
            );
        }
    }
}