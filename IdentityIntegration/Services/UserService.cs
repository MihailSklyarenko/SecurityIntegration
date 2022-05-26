using Microsoft.EntityFrameworkCore;
using SecurityIntegration.Database.IdentityContext;
using SecurityIntegration.Database.IdentityEntries;
using SecurityIntegration.Jwt;
using SecurityIntegration.Managers;
using SecurityIntegration.Models;

namespace SecurityIntegration.Services;

public class UserService : IUserService
{
	private readonly IdentityContext _context;
	private readonly AppUserManager _userManager;
	private readonly AppSignInManager _signInManager;
	private readonly IJwtGenerator _jwtGenerator;

    public UserService(IdentityContext context,
        AppUserManager userManager,
        AppSignInManager signInManager,
        IJwtGenerator jwtGenerator)
    {
        _context = context;
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _signInManager = signInManager;
    }	

	public async Task<UserDto> Login(UserLoginRequest request, CancellationToken token)
    {
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			throw new Exception("User not found");
		}

		var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

		if (result.Succeeded)
		{
			return new UserDto
			{
				DisplayName = user.DisplayName,
				Token = _jwtGenerator.CreateToken(user),
				UserName = user.UserName
			};
		}

		throw new Exception("Login failed");
	}

	public async Task LogOut(UserLoginRequest request, CancellationToken token)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			throw new Exception("User not found");
		}

		await _signInManager.SignOutAsync();
	}

	public async Task<UserDto> Register(UserCreateRequest request, CancellationToken token)
	{
		if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(token))
		{
			throw new Exception("Email already exist");
		}

		if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync(token))
		{
			throw new Exception("UserName already exist");
		}

		var user = new User
		{
			DisplayName = request.DisplayName,
			Email = request.Email,
			UserName = request.UserName
		};

		var result = await _userManager.CreateAsync(user, request.Password);

		if (result.Succeeded)
		{
			return new UserDto
			{
				DisplayName = user.DisplayName,
				Token = _jwtGenerator.CreateToken(user),
				UserName = user.UserName
			};
		}

		throw new Exception("Client creation failed");
	}
}