using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecurityIntegration.Models;
using SecurityIntegration.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost("register")]
        public async Task<UserDto> Register(UserCreateRequest request, CancellationToken token = default)
        {
            return await _userService.Register(request);
        }

        [HttpPost("login")]
        public async Task<UserDto> Login(UserLoginRequest request, CancellationToken token = default)
        {
            return await _userService.Login(request);
        }
    }
}
