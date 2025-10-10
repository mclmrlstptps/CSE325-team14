using Microsoft.AspNetCore.Mvc;
using RestaurantMS.Models;
using RestaurantMS.Services;
using System.Threading.Tasks;

namespace RestaurantMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password, string name, string role)
        {
            try
            {
                var result = await _authService.RegisterAsync(email, password, name, role);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var result = await _authService.LoginAsync(email, password);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
