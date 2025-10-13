using RestaurantMS.Models;
using RestaurantMS.Services;
using System.Threading.Tasks;
using BCrypt.Net;

namespace RestaurantMS.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<bool> RegisterAsync(ApplicationUser newUser, string plainPassword)
        {
            var existingUser = await _userService.GetUserByEmailAsync(newUser.Email);
            if (existingUser != null) return false;

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            await _userService.CreateUserAsync(newUser);
            return true;
        }
    }
}
