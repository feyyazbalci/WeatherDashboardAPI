using Microsoft.AspNetCore.Mvc;
using WeatherDashboardAPI.Services;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current/{cityId}")]
        public async Task<IActionResult> GetCurrentWeather(int cityId)
        {
            var (success, message, weather) = await _weatherService.GetCurrentWeatherAsync(cityId);

            if (!success)
                return NotFound(new { message });

            return Ok(new { message, data = weather });
        }

        [HttpPost("sync/{cityId}")]
        public async Task<IActionResult> SyncWeatherData(int cityId)
        {
            var (success, message) = await _weatherService.SyncWeatherDataAsync(cityId);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("sync-all")]
        public async Task<IActionResult> SyncAllCitiesWeather()
        {
            var (success, message) = await _weatherService.SyncAllCitiesWeatherAsync();

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetWeatherHistory(
            [FromQuery] int? cityId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _weatherService.GetWeatherHistoryAsync(cityId, startDate, endDate, page, pageSize);

            return Ok(new
            {
                data = result.Data,
                pagination = new
                {
                    totalCount = result.TotalCount,
                    page = result.Page,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages,
                    hasPrevious = result.HasPrevious,
                    hasNext = result.HasNext
                }
            });
        }
    }
}