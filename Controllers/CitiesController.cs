using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.Services;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null)
        {
            var result = await _cityService.GetAllCitiesAsync(page, pageSize, search, sortBy);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id)
        {
            var city = await _cityService.GetCityByIdAsync(id);

            if (city == null)
                return NotFound(new { message = "City not found." });

            return Ok(city);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, city) = await _cityService.CreateCityAsync(dto);

            if (!success)
                return BadRequest(new { message });

            return CreatedAtAction(nameof(GetCity), new { id = city!.Id }, new { message, data = city });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, city) = await _cityService.UpdateCityAsync(id, dto);

            if (!success)
                return NotFound(new { message });

            return Ok(new { message, data = city });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var (success, message) = await _cityService.DeleteCityAsync(id);

            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }


    }
}