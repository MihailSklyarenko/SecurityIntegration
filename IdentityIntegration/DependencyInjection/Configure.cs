using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecurityIntegration.Configutarion.Jwt;
using SecurityIntegration.Database.IdentityEntries;
using SecurityIntegration.Jwt;
using SecurityIntegration.Managers;
using SecurityIntegration.Services;
using SecurityIntegration.Stores;
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
    }

    public static void IntegrateJWT(this WebApplicationBuilder builder)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtOptions:Secret").Value));
        builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("JwtOptions:Issuer").Value,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        //ClockSkew = TimeSpan.Zero
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

    public static void IntegrateSwaggerWithBearerAuth(this WebApplicationBuilder builder, string title, string version)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                                Enter 'Bearer' [space] and then your token in the text input below.
                                \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.DescribeAllParametersInCamelCase();

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }
}