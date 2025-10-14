using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace RestaurantMS.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Try to get user from session storage
                var userSessionResult = await _sessionStorage.GetAsync<UserSession>("userSession");
                
                if (userSessionResult.Success && userSessionResult.Value != null)
                {
                    var userSession = userSessionResult.Value;
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userSession.Email),
                        new Claim(ClaimTypes.Email, userSession.Email),
                        new Claim(ClaimTypes.Role, userSession.Role),
                        new Claim("UserId", userSession.UserId ?? "")
                    }, "apiauth");

                    var user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
            }
            catch
            {
                // If there's any error reading from storage, return anonymous
            }

            return new AuthenticationState(_anonymous);
        }

        public async Task MarkUserAsAuthenticated(string email, string role, string? userId = null, string? name = null)
        {
            var userSession = new UserSession
            {
                Email = email,
                Role = role,
                UserId = userId,
                Name = name ?? email
            };

            await _sessionStorage.SetAsync("userSession", userSession);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name ?? email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", userId ?? "")
            }, "apiauth");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.DeleteAsync("userSession");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        // Helper class to store user session
        private class UserSession
        {
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string? UserId { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
