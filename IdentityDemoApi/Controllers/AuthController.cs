using Microsoft.AspNetCore.Mvc;
using SecurityIntegration.Models;
using SecurityIntegration.Services;

namespace IdentityDemoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<UserDto> Login(UserLoginRequest request, CancellationToken token = default)
        {
            return await _userService.Login(request, token);
        }

        [HttpPost("logout")]
        public async Task Logout(UserLoginRequest request, CancellationToken token = default)
        {            
            await _userService.LogOut(request, token);
        }

        [HttpPost("register")]
        public async Task<UserDto> Register(UserCreateRequest request, CancellationToken token = default)
        {
            return await _userService.Register(request, token);
        }
    }
}
