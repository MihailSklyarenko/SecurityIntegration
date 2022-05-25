using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SecurityIntegration.Configutarion.Jwt;
using SecurityIntegration.Database.IdentityEntries;
using SecurityIntegration.Jwt;
using SecurityIntegration.Managers;
using SecurityIntegration.Stores;
using System.Text;

namespace SecurityIntegration.DependencyInjection;

public static class ConfigureIdentity
{
    public static void IntegrateIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddIdentity<User, Role>()
            .AddUserManager<AppUserManager>()
            .AddUserStore<AppUserStore>()
            .AddRoleStore<AppRoleStore>()
            .AddRoleManager<AppRoleManager>()
            .AddSignInManager<AppSignInManager>()
            .AddDefaultTokenProviders();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });

        builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
    }
}
