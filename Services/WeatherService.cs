using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherDashboardAPI.DTOs.Weather;
using WeatherDashboardAPI.Helpers;
using WeatherDashboardAPI.Models;
using WeatherDashboardAPI.Models.OpenWeatherMap;
using WeatherDashboardAPI.Repositories;

namespace WeatherDashboardAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, CurrentWeatherDto? Weather)> GetCurrentWeatherAsync(int cityId)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(cityId);
            if (city == null)
                return (false, "City not found.", null);

            var latestRecord = await _unitOfWork.WeatherRecords
                .GetQueryable()
                .Where(w => w.CityId == cityId)
                .OrderByDescending(w => w.RecordedAt)
                .Include(w => w.City)
                .FirstOrDefaultAsync();

            if (latestRecord == null || latestRecord.RecordedAt < DateTime.UtcNow.AddMinutes(-30))
            {
                var syncResult = await SyncWeatherDataAsync(cityId);
                if (!syncResult.Success)
                    return (false, syncResult.Message, null);

                latestRecord = await ((Repository<WeatherRecord>)_unitOfWork.WeatherRecords)
                    .GetQueryable()
                    .Where(w => w.CityId == cityId)
                    .OrderByDescending(w => w.RecordedAt)
                    .Include(w => w.City)
                    .FirstOrDefaultAsync();
            }

            if (latestRecord == null)
                return (false, "Weather data could not be obtained.", null);

            var weatherDto = _mapper.Map<CurrentWeatherDto>(latestRecord);
            return (true, "Weather forecast successfully brought.", weatherDto);
        }

        public async Task<(bool Success, string Message)> SyncWeatherDataAsync(int cityId)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(cityId);
            if (city == null)
                return (false, "City not found.");

            try
            {
                var weatherData = await FetchWeatherFromApiAsync(city.Latitude, city.Longitude);
                if (weatherData == null)
                    return (false, "Weather data could not be obtained.");

                var weatherRecord = _mapper.Map<WeatherRecord>(weatherData);
                weatherRecord.CityId = cityId;

                await _unitOfWork.WeatherRecords.AddAsync(weatherRecord);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Weather data has been recorded successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }
        public async Task<(bool Success, string Message)> SyncAllCitiesWeatherAsync()
        {
            var cities = await _unitOfWork.Cities.GetAllAsync();
            var cityList = cities.ToList();

            if (!cityList.Any())
                return (false, "No cities found.");

            int successCount = 0;
            int failCount = 0;

            foreach (var city in cityList)
            {
                var result = await SyncWeatherDataAsync(city.Id);
                if (result.Success)
                    successCount++;
                else
                    failCount++;

                await Task.Delay(1000);
            }

            return (true, $"Toplam {cityList.Count} şehir: {successCount} başarılı, {failCount} başarısız.");
        }

        public async Task<PagedResult<WeatherRecordDto>> GetWeatherHistoryAsync(
            int? cityId,
            DateTime? startDate,
            DateTime? endDate,
            int page,
            int pageSize)
        {
            // Validation
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize > 100 ? 100 : pageSize;

            // Query

            var query = _unitOfWork.WeatherRecords.GetQueryable();

            // Filters
            if (cityId.HasValue)
                query = query.Where(w => w.CityId == cityId.Value);

            if (startDate.HasValue)
                query = query.Where(w => w.RecordedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(w => w.RecordedAt <= endDate.Value);

            query = query.OrderByDescending(w => w.RecordedAt);

            query = query.Include(w => w.City);

            // Total count
            var totalCount = await query.CountAsync();

            // Pagination
            var records = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTO
            var recordDtos = _mapper.Map<List<WeatherRecordDto>>(records);

            return new PagedResult<WeatherRecordDto>
            {
                Data = recordDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private async Task<OpenWeatherResponse?> FetchWeatherFromApiAsync(double lat, double lon)
        {
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var baseUrl = _configuration["OpenWeatherMap:BaseUrl"];

            var client = _httpClientFactory.CreateClient();
            var url = $"{baseUrl}/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=tr";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(json);

            return weatherData;
        }
        


    }
}