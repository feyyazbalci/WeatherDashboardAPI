using WeatherDashboardAPI.Repositories;

namespace WeatherDashboardAPI.Services
{
    public class WeatherUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherUpdateBackgroundService> _logger;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(30);

        public WeatherUpdateBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<WeatherUpdateBackgroundService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var intervalMinutes = configuration.GetValue<int>("BackgroundService:UpdateIntervalMinutes");
            _updateInterval = TimeSpan.FromMinutes(intervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather Update Background Service is started.");

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Weather update process has started: {Time}", DateTime.UtcNow);

                    await UpdateAllCitiesWeatherAsync();

                    _logger.LogInformation("Weather update completed: {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the weather forecast: {Message}", ex.Message);
                }

                _logger.LogInformation("Next update: {Time}", DateTime.UtcNow.Add(_updateInterval));
                await Task.Delay(_updateInterval, stoppingToken);
            }

            _logger.LogInformation("Weather Update Background Service is stopping.");
        }

        private async Task UpdateAllCitiesWeatherAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();

            var cities = await unitOfWork.Cities.GetAllAsync();
            var cityList = cities.ToList();

            _logger.LogInformation("A total of {Count} cities will be updated.", cityList.Count);

            int successCount = 0;
            int failCount = 0;

            foreach (var city in cityList)
            {
                try
                {
                    _logger.LogInformation("Updating: {CityName}, {Country}", city.Name, city.Country);

                    var result = await weatherService.SyncWeatherDataAsync(city.Id);

                    if (result.Success)
                    {
                        successCount++;
                        _logger.LogInformation("Successful: {CityName}", city.Name);
                    }
                    else
                    {
                        failCount++;
                        _logger.LogWarning("Unsuccessful: {CityName} - {Message}", city.Name, result.Message);
                    }

                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    failCount++;
                    _logger.LogError(ex, "Error: {CityName} - {Message}", city.Name, ex.Message);
                }

                _logger.LogInformation(
                "Update summary: Total: {Total}, Success: {Success}, Failure: {Fail}",
                cityList.Count, successCount, failCount);
            }

        }
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather Update Background Service stopping...");
            await base.StopAsync(stoppingToken);
        }

    }
}