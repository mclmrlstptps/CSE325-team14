using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestaurantMS.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            _jwtSecret = _configuration["Jwt:Secret"] ?? "RestaurantMS_SuperSecretKey_2024_ForJWTTokenGeneration_AtLeast32Chars!";
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "RestaurantMS";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "RestaurantMS";
        }

        public async Task<AuthResponse?> RegisterAsync(string email, string password, string name, string role)
        {
            if (role != "Manager" && role != "Employee")
                throw new ArgumentException("Role must be either 'Manager' or 'Employee'");

            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            var user = new User
            {
                Email = email.ToLowerInvariant(),
                PasswordHash = HashPassword(password),
                Name = name,
                Role = role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userService.CreateUserAsync(user);

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new UserInfo
                {
                    Id = user.Id ?? "",
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                }
            };
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email.ToLowerInvariant());
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            if (!user.IsActive)
                throw new InvalidOperationException("Account is deactivated");

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new UserInfo
                {
                    Id = user.Id ?? "",
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                }
            };
        }

        private string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
