using WeatherDashboardAPI.DTOs.Weather;
using WeatherDashboardAPI.Helpers;

namespace WeatherDashboardAPI.Services
{
    public interface IWeatherService
    {
        Task<(bool Success, string Message, CurrentWeatherDto? Weather)> GetCurrentWeatherAsync(int cityId);
        Task<(bool Success, string Message)> SyncWeatherDataAsync(int cityId);
        Task<(bool Success, string Message)> SyncAllCitiesWeatherAsync();

        Task<PagedResult<WeatherRecordDto>> GetWeatherHistoryAsync(
            int? cityId, 
            DateTime? startDate, 
            DateTime? endDate,
            int page, 
            int pageSize);
    }
}