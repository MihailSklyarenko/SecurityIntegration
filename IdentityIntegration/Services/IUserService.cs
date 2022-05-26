using SecurityIntegration.Models;

namespace SecurityIntegration.Services;

public interface IUserService
{
    Task<UserDto> Login(UserLoginRequest request, CancellationToken token);
    Task LogOut(UserLoginRequest request, CancellationToken token);
    Task<UserDto> Register(UserCreateRequest request, CancellationToken token);
}
