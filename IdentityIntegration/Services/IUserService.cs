using SecurityIntegration.Models;

namespace SecurityIntegration.Services;

public interface IUserService
{
    Task<UserDto> Register(UserCreateRequest request);
    Task<UserDto> Login(UserLoginRequest request);
}
