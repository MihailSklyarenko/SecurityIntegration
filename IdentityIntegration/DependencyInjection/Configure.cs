using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SecurityIntegration.Configutarion.Jwt;
using SecurityIntegration.Database.IdentityContext;
using SecurityIntegration.Database.IdentityEntries;
using SecurityIntegration.Jwt;
using SecurityIntegration.Managers;
using SecurityIntegration.Services;
using SecurityIntegration.Stores;
using System.Security.Claims;
using System.Text;

namespace SecurityIntegration.DependencyInjection;

public static class Configure
{
    public static void IntegrateIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
        })
            .AddUserManager<AppUserManager>()
            .AddUserStore<AppUserStore>()
            .AddRoleStore<AppRoleStore>()
            .AddRoleManager<AppRoleManager>()
            .AddSignInManager<AppSignInManager>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<IUserService, UserService>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtOptions:Secret").Value));
        builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = key
                    };
                });

        builder.Services
            .AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("default", defaultPolicy);
                options.DefaultPolicy = defaultPolicy;
            });

        builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
    }
}
