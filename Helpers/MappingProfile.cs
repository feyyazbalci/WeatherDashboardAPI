using AutoMapper;
using WeatherDashboardAPI.Models;
using WeatherDashboardAPI.DTOs.Auth;
using WeatherDashboardAPI.DTOs.City;
using WeatherDashboardAPI.DTOs.Weather;
using WeatherDashboardAPI.DTOs.Favorite;
using WeatherDashboardAPI.DTOs.Alert;
using WeatherDashboardAPI.Models.OpenWeatherMap;
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

            CreateMap<OpenWeatherResponse, WeatherRecord>()
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Main!.Temp))
                .ForMember(dest => dest.FeelsLike, opt => opt.MapFrom(src => src.Main!.FeelsLike))
                .ForMember(dest => dest.TempMin, opt => opt.MapFrom(src => src.Main!.TempMin))
                .ForMember(dest => dest.TempMax, opt => opt.MapFrom(src => src.Main!.TempMax))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Main!.Humidity))
                .ForMember(dest => dest.Pressure, opt => opt.MapFrom(src => src.Main!.Pressure))
                .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.Wind != null ? src.Wind.Speed : 0))
                .ForMember(dest => dest.WindDegree, opt => opt.MapFrom(src => src.Wind != null ? src.Wind.Deg : (int?)null))
                .ForMember(dest => dest.Cloudiness, opt => opt.MapFrom(src => src.Clouds != null ? src.Clouds.All : (int?)null))
                .ForMember(dest => dest.Rainfall, opt => opt.MapFrom(src => 
                    src.Rain != null ? (src.Rain.OneHour ?? src.Rain.ThreeHours) : null))
                .ForMember(dest => dest.Snowfall, opt => opt.MapFrom(src => 
                    src.Snow != null ? (src.Snow.OneHour ?? src.Snow.ThreeHours) : null))
                .ForMember(dest => dest.WeatherMain, opt => opt.MapFrom(src => 
                    src.Weather != null && src.Weather.Any() ? src.Weather[0].Main : "Unknown"))
                .ForMember(dest => dest.WeatherDescription, opt => opt.MapFrom(src => 
                    src.Weather != null && src.Weather.Any() ? src.Weather[0].Description : "No description"))
                .ForMember(dest => dest.WeatherIcon, opt => opt.MapFrom(src => 
                    src.Weather != null && src.Weather.Any() ? src.Weather[0].Icon : null))
                .ForMember(dest => dest.RecordedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CityId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());
            
        }
    }
} 