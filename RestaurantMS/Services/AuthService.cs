using RestaurantMS.Models;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace RestaurantMS.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly ProtectedSessionStorage _sessionStorage;

        public AuthService(UserService userService, ProtectedSessionStorage sessionStorage)
        {
            _userService = userService;
            _sessionStorage = sessionStorage;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return false;

            var verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!verified) return false;

            await _sessionStorage.SetAsync("currentUser", user);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _sessionStorage.DeleteAsync("currentUser");
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var result = await _sessionStorage.GetAsync<ApplicationUser>("currentUser");
            return result.Success && result.Value != null;
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var result = await _sessionStorage.GetAsync<ApplicationUser>("currentUser");
            return result.Success ? result.Value : null;
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
