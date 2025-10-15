using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantMS.Services;

namespace RestaurantMS.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly CustomAuthStateProvider _authStateProvider;

        public LogoutModel(CustomAuthStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task OnGetAsync()
        {
            await _authStateProvider.MarkUserAsLoggedOut();
            Response.Redirect("/");
        }
    }
}
