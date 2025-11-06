using AutoMapper;
using WeatherDashboardAPI.Models;
using WeatherDashboardAPI.DTOs.Auth;
using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.DTOs.Weather;
using WeatherDashboardAPI.DTOs.Favorite;
using WeatherDashboardAPI.DTOs.Alert;
using WeatherDashboardAPI.Data;

namespace WeatherDashboardAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<City, CityDto>();
            CreateMap<CreateCityDto, City>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.OpenWeatherMapId, opt => opt.Ignore())
                .ForMember(dest => dest.WeatherRecords, opt => opt.Ignore())
                .ForMember(dest => dest.FavoritedByUsers, opt => opt.Ignore())
                .ForMember(dest => dest.WeatherAlerts, opt => opt.Ignore());

            CreateMap<UpdateCityDto, City>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.OpenWeatherMapId, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<WeatherRecord, WeatherRecordDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country));

            CreateMap<WeatherRecord, CurrentWeatherDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.RecordedAt));

            CreateMap<UserFavoriteCity, FavoriteCityDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country));

            CreateMap<WeatherAlert, WeatherAlertDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.AlertType, opt => opt.MapFrom(src => src.AlertType.ToString()))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity.ToString()));
            
        }
    }
} 