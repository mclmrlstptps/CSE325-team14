using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RestaurantMS.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantMS.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await _sessionStorage.GetAsync<ApplicationUser>("currentUser");
            ClaimsIdentity identity;

            if (result.Success && result.Value != null)
            {
                var user = result.Value;
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }, "SessionAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var userPrincipal = new ClaimsPrincipal(identity);
            return new AuthenticationState(userPrincipal);
        }

        public void NotifyUserAuthentication(ApplicationUser user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }, "SessionAuth");

            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void NotifyUserLogout()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }
    }
}
