using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecurityIntegration.Configutarion.Jwt;
using SecurityIntegration.Database.IdentityEntries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecurityIntegration.Jwt;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtOptions _options;
    private readonly SymmetricSecurityKey _key;

    public JwtGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim> 
        { 
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_options.ExpireInMinutes),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
