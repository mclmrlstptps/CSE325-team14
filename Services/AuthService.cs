using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestaurantMS.Models;
using BCrypt.Net;

namespace RestaurantMS.Services
{
    public class AuthService
    {
        private readonly MongoDBService _mongoService;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AuthService(MongoDBService mongoService, IConfiguration configuration)
        {
            _mongoService = mongoService;
            _configuration = configuration;
            _jwtSecret = _configuration["Jwt:Secret"] ?? "";
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "RestaurantMS";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "RestaurantMS";
        }
        public async Task<AuthResponse?> RegisterAsync(string email, string password, string name, string role)
        {
            try
            {
                // check role, manager or employee
                if (role != "Manager" && role != "Employee")
                {
                    throw new ArgumentException("Role must be either 'Manager' or 'Employee'");
                }

                // check if user exists
                var existingUser = await _mongoService.GetUserByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("User with this email already exists");
                }

                // create new user
                var user = new User
                {
                    Email = email.ToLowerInvariant(),
                    PasswordHash = HashPassword(password),
                    Name = name,
                    Role = role,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // save to database
                await _mongoService.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                throw;
            }

        }

    }
}
