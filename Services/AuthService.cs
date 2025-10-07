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

       
    }
}
