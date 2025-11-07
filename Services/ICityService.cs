using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.Helpers;

namespace WeatherDashboardAPI.Services
{
    public interface ICityService
    {
        Task<PagedResult<CityDto>> GetAllCitiesAsync(int page, int pageSize, string? search, string? sortBy);
        Task<CityDto?> GetCityByIdAsync(int id);
        Task<(bool Success, string Message, CityDto? City)> CreateCityAsync(CreateCityDto dto);
        Task<(bool Success, string Message, CityDto? City)> UpdateCityAsync(int id, UpdateCityDto dto);
        Task<(bool Success, string Message)> DeleteCityAsync(int id);
    }
}