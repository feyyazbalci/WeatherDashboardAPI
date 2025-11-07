using WeatherDashboardAPI.Data;
using WeatherDashboardAPI.DTOs.Auth;

namespace WeatherDashboardAPI.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, UserDto? User)> RegisterAsync(RegisterRequestDto dto);
        Task<(bool Success, string Message, LoginResponseDto? Response)> LoginAsync(LoginRequestDto dto);
        Task<UserDto?> GetUserByIdAsync(int userId);

    }
}