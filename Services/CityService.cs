using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.Helpers;
using WeatherDashboardAPI.Models;
using WeatherDashboardAPI.Repositories;

namespace WeatherDashboardAPI.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<CityDto>> GetAllCitiesAsync(int page, int pageSize, string? search, string? sortBy)
        {
            // Validation
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize > 100 ? 100 : pageSize;

            // Base query
            var query = _unitOfWork.Cities.GetQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.Country.ToLower().Contains(search));
            }

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(c => c.Name),
                "name_desc" => query.OrderByDescending(c => c.Name),
                "country" => query.OrderBy(c => c.Country).ThenBy(c => c.Name),
                "country_desc" => query.OrderByDescending(c => c.Country).ThenBy(c => c.Name),
                "created" => query.OrderBy(c => c.CreatedAt),
                "created_desc" => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderBy(c => c.Name) // Default
            };

            // Total count
            var totalCount = await query.CountAsync();

            // Pagination
            var cities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTO
            var cityDtos = _mapper.Map<List<CityDto>>(cities);

            return new PagedResult<CityDto>
            {
                Data = cityDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<CityDto?> GetCityByIdAsync(int id)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(id);

            if (city == null)
                return null;

            return _mapper.Map<CityDto>(city);
        }

        public async Task<(bool Success, string Message, CityDto? City)> CreateCityAsync(CreateCityDto dto)
        {
            var existingCity = await _unitOfWork.Cities.FirstOrDefaultAsync(c =>
                c.Name.ToLower() == dto.Name.ToLower() &&
                c.Country.ToUpper() == dto.Country.ToUpper()
            );

            if (existingCity != null)
            {
                return (false, "This city already exists", null);
            }

            var city = _mapper.Map<City>(dto);

            await _unitOfWork.Cities.AddAsync(city);
            await _unitOfWork.SaveChangesAsync();

            var cityDto = _mapper.Map<CityDto>(city);

            return (true, "City successfully created.", cityDto);
        }

        public async Task<(bool Success, string Message, CityDto? City)> UpdateCityAsync(int id, UpdateCityDto dto)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(id);

            if (city == null)
                return (false, "City not found.", null);

            if (!string.IsNullOrWhiteSpace(dto.Name))
                city.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Country))
                city.Country = dto.Country;

            if (dto.Latitude.HasValue)
                city.Latitude = dto.Latitude.Value;

            if (dto.Longitude.HasValue)
                city.Longitude = dto.Longitude.Value;

            if (dto.StateCode != null)
                city.StateCode = dto.StateCode;

            _unitOfWork.Cities.Update(city);
            await _unitOfWork.SaveChangesAsync();

            var cityDto = _mapper.Map<CityDto>(city);

            return (true, "City successfully updated.", cityDto);
        }

        public async Task<(bool Success, string Message)> DeleteCityAsync(int id)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(id);

            if (city == null)
            {
                return (false, "City not found.");
            }

            _unitOfWork.Cities.Remove(city);
            await _unitOfWork.SaveChangesAsync();

            return (true, "The city was successfully deleted.");
        }
        
    }
}