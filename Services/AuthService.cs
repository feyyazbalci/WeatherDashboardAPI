using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherDashboardAPI.Data;
using WeatherDashboardAPI.DTOs.Auth;
using WeatherDashboardAPI.Helpers;
using WeatherDashboardAPI.Models;

namespace WeatherDashboardAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public AuthService(ApplicationDbContext context, IMapper mapper, JwtHelper jwtHelper)
        {
            _context = context;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        public async Task<(bool Success, string Message, UserDto? User)> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return (false, "Email is already registered.", null);
            }

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return (false, "Username is already taken.", null);
            }

            var user = _mapper.Map<User>(dto);

            user.PasswordHash = PasswordHelper.HashPassword(dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            return (true, "Successfully registered.", userDto);
        }

        public async Task<(bool Success, string Message, LoginResponseDto? Response)> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null);
            }

            if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return (false, "Email or Password are wrong.", null);
            }

            var token = _jwtHelper.GenerateToken(user);

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var jwtSettings = _jwtHelper._configuration.GetSection("JwtSettings");
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "1440");

            var response = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };

            return (true, "Login successful.", response);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                return null;

            return _mapper.Map<UserDto>(user);
        }

    }
}