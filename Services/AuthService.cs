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

    }
}
