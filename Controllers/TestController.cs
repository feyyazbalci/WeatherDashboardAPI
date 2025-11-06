using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherDashboardAPI.Data;
using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TestController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            var cities = await _context.Cities.ToListAsync();
            var cityDtos = _mapper.Map<List<CityDto>>(cities);
            return Ok(cityDtos);
        }

        [HttpGet("cities/{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound(new { Message = "City not found." });
            }

            var cityDto = _mapper.Map<CityDto>(city);
            return Ok(cityDto);
        }

        [HttpPost("cities")]
        public async Task<ActionResult<CityDto>> CreateCity(CreateCityDto dto)
        {
            var city = _mapper.Map<City>(dto);

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var cityDto = _mapper.Map<CityDto>(city);

            return CreatedAtAction(nameof(GetCities), new { id = city.Id}, cityDto);
        }
    }
}