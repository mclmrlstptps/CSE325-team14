using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace RestaurantMS.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_anonymous));
        }

        public Task MarkUserAsAuthenticated(string email, string role)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return Task.CompletedTask;
        }

        public Task MarkUserAsLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
            return Task.CompletedTask;
        }
    }
}
